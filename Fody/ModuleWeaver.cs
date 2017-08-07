using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace JetBrainsAnnotations.Fody
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using JetBrains.Annotations;

    using Mono.Collections.Generic;

    public class ModuleWeaver
    {
        [NotNull, UsedImplicitly]
        public Action<string> LogInfo { get; set; }
        [NotNull, UsedImplicitly]
        public Action<string> LogWarning { get; set; }
        [NotNull, UsedImplicitly]
        public Action<string> LogError { get; set; }
        [NotNull, UsedImplicitly]
        public ModuleDefinition ModuleDefinition { get; set; }
        [NotNull, UsedImplicitly]
        public IAssemblyResolver AssemblyResolver { get; set; }
        [NotNull, UsedImplicitly]
        public string ProjectDirectoryPath { get; set; }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public ModuleWeaver()
        {
            LogInfo = LogWarning = LogError = _ => { };
        }

        public void Execute()
        {
            var assemblyReferences = AssemblyReferences;
            // ReSharper disable once PossibleNullReferenceException
            var jetbrainsAnnotationsReference = assemblyReferences.FirstOrDefault(x => x.Name.StartsWith("JetBrains.Annotations"));
            if (jetbrainsAnnotationsReference == null)
            {
                LogWarning("Reference to JetBrains.Annotations not found.");
                return;
            }

            if (string.IsNullOrEmpty(ProjectDirectoryPath) || !Directory.Exists(ProjectDirectoryPath))
            {
                LogError("ProjectDirectoryPath is not a valid directory: " + ProjectDirectoryPath);
                return;
            }

            assemblyReferences.Remove(jetbrainsAnnotationsReference);

            var jetbrainsAnnotations = AssemblyResolver.Resolve(jetbrainsAnnotationsReference);

            Debug.Assert(jetbrainsAnnotations != null, "jetbrainsAnnotations != null");
            var elements = new Executor(ModuleDefinition, jetbrainsAnnotations).Execute();

            if (!elements.Any())
                return;

            Save(elements);
        }

        [NotNull, ItemNotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        private Collection<AssemblyNameReference> AssemblyReferences => ModuleDefinition.AssemblyReferences;

        private void Save([NotNull, ItemNotNull] IEnumerable<XElement> elements)
        {
            var assemblyName = ModuleDefinition.Assembly?.Name?.Name;
            Debug.Assert(assemblyName != null, "assemblyName != null");

            var externalAnnotationsFileName = assemblyName + ".ExternalAnnotations.xml";
            var targetName = Path.Combine(ProjectDirectoryPath, externalAnnotationsFileName);

            LogInfo($"Generate external annotations for {assemblyName} => {externalAnnotationsFileName} => {targetName}");

            var document = new XDocument();
            var root = new XElement("assembly", new XAttribute("name", assemblyName));

            root.Add(elements.OrderBy(el => el.Attribute("name")?.Value.Substring(2)));

            document.Add(root);

            if (ContentEquals(targetName, document))
            {
                LogInfo(targetName + " is already up to date.");
                return;
            }

            LogInfo("Updated " + targetName);
            document.Save(targetName);
        }

        private static bool ContentEquals([NotNull] string targetName, [NotNull] XNode document)
        {
            try
            {
                return File.Exists(targetName)
                       && XDocument.Load(targetName).ToString(SaveOptions.DisableFormatting) == document.ToString(SaveOptions.DisableFormatting);
            }
            catch
            {
                // error reading the file, assume not equal
            }

            return false;
        }

        private class Executor
        {
            [NotNull, ItemNotNull]
            private readonly ICollection<string> _allAttributes;
            [NotNull, ItemNotNull]
            private readonly IList<XElement> _root;
            [NotNull]
            private readonly ModuleDefinition _moduleDefinition;

            public Executor([NotNull] ModuleDefinition moduleDefinition, [NotNull] AssemblyDefinition jetbrainsAnnotations)
            {
                _moduleDefinition = moduleDefinition;
                _root = new List<XElement>();
                _allAttributes = new HashSet<string>(GetAllAttributes(jetbrainsAnnotations));
            }

            [NotNull, ItemNotNull]
            [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public IList<XElement> Execute()
            {
                TrimAttributes(_moduleDefinition);
                TrimAttributes(_moduleDefinition.Assembly);

                foreach (var type in _moduleDefinition.GetTypes())
                {
                    ProcessAttributes(type, type.IsPublicVisible());

                    foreach (var parameter in type.GenericParameters)
                    {
                        TrimAttributes(parameter);
                    }

                    foreach (var field in type.Fields)
                    {
                        ProcessAttributes(field, field.IsPublicVisible());
                    }

                    foreach (var anEvent in type.Events)
                    {
                        ProcessAttributes(anEvent, anEvent.IsPublicVisible());
                    }

                    foreach (var property in type.Properties)
                    {
                        ProcessAttributes(property, property.IsPublicVisible());
                    }

                    foreach (var method in type.Methods)
                    {
                        ProcessMethodAttributes(method);
                    }
                }

                return _root;
            }

            private void ProcessAttributes([NotNull] IMemberDefinition member, bool isPublic)
            {
                var attributes = TrimAttributes(member);
                if (!isPublic || !attributes.Any())
                    return;


                var id = DocCommentId.GetDocCommentId(member);
                // ReSharper disable once AssignNullToNotNullAttribute
                var content = new XElement("member", new XAttribute("name", id));

                foreach (var attribute in attributes)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    // ReSharper disable once AssignNullToNotNullAttribute
                    content.Add(new XElement("attribute", new XAttribute("ctor", DocCommentId.GetDocCommentId(attribute.Constructor.Resolve()))));
                }

                _root.Add(content);
            }

            private void ProcessMethodAttributes([NotNull] MethodDefinition method)
            {
                var attributes = TrimAttributes(method);
                var isPublicVisible = method.IsPublicVisible();

                // ReSharper disable once AssignNullToNotNullAttribute
                TrimAttributes(method.MethodReturnType);

                var id = DocCommentId.GetDocCommentId(method);
                Debug.Assert(id != null, "id != null");
                var content = new XElement("member", new XAttribute("name", id));

                foreach (var attribute in attributes)
                {
                    AddAttributeDocumentation(content, attribute);
                }

                ProcessParameterAttributes(method, content, isPublicVisible);

                // ReSharper disable once PossibleNullReferenceException
                foreach (var parameter in method.GenericParameters)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    TrimAttributes(parameter);
                }

                if (isPublicVisible && content.HasElements)
                {
                    _root.Add(content);
                }
            }

            private void ProcessParameterAttributes([NotNull] MethodDefinition method, [NotNull] XElement parent, bool isPublicVisible)
            {
                // ReSharper disable once PossibleNullReferenceException
                foreach (var parameter in method.Parameters)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var parameterAttributes = TrimAttributes(parameter);

                    if (!isPublicVisible)
                        continue;

                    // ReSharper disable once AssignNullToNotNullAttribute
                    var content = new XElement("parameter", new XAttribute("name", parameter.Name));

                    foreach (var attribute in parameterAttributes)
                    {
                        AddAttributeDocumentation(content, attribute);
                    }

                    if (content.HasElements)
                    {
                        parent.Add(content);
                    }
                }
            }

            private void AddAttributeDocumentation([NotNull] XElement content, [NotNull] CustomAttribute attribute)
            {
                // ReSharper disable once PossibleNullReferenceException
                var constructor = attribute.Constructor.Resolve();

                var id = DocCommentId.GetDocCommentId(constructor);

                // ReSharper disable once AssignNullToNotNullAttribute
                var element = new XElement("attribute", new XAttribute("ctor", id));

                // ReSharper disable once PossibleNullReferenceException
                foreach (var argument in attribute.ConstructorArguments)
                {
                    element.Add(new XElement("argument", argument.Value?.ToString()));
                }

                content.Add(element);
            }

            [NotNull, ItemNotNull]
            private IList<CustomAttribute> TrimAttributes([NotNull] ICustomAttributeProvider attributeProvider)
            {
                var customAttributes = attributeProvider.CustomAttributes;
                Debug.Assert(customAttributes != null, "customAttributes != null");
                var attributes = customAttributes
                    .Where(attr => _allAttributes.Contains(attr?.AttributeType?.Name))
                    .ToArray();

                foreach (var attribute in attributes)
                {
                    customAttributes.Remove(attribute);
                }

                return attributes;
            }

            [NotNull, ItemNotNull]
            private static IEnumerable<string> GetAllAttributes([NotNull] AssemblyDefinition assemblyDefinition)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                return assemblyDefinition.MainModule?.Types?
                    .Select(type => type?.Name)
                    .Where(attributeName => attributeName?.EndsWith("Attribute") == true);
            }
        }
    }
}