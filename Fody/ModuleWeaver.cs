using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace JetBrainsAnnotations.Fody
{
    using JetBrains.Annotations;

    public class ModuleWeaver
    {
        public Action<string> LogInfo { get; set; }
        public Action<string> LogWarning { get; set; }
        public Action<string> LogError { get; set; }
        public ModuleDefinition ModuleDefinition { get; set; }
        public IAssemblyResolver AssemblyResolver { get; set; }
        public string ProjectDirectoryPath { get; set; }

        public ModuleWeaver()
        {
            LogInfo = LogWarning = LogError = _ => { };
        }

        public void Execute()
        {
            var jetbrainsAnnotationsReference = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name.StartsWith("JetBrains.Annotations"));
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

            ModuleDefinition.AssemblyReferences.Remove(jetbrainsAnnotationsReference);

            var jetbrainsAnnotations = AssemblyResolver.Resolve(jetbrainsAnnotationsReference);

            var elements = new Executor(ModuleDefinition, jetbrainsAnnotations).Execute();

            if (!elements.Any())
                return;

            Save(elements);
        }

        private void Save(IList<XElement> elements)
        {
            var targetName = Path.Combine(ProjectDirectoryPath, Path.ChangeExtension(Path.GetFileName(ModuleDefinition.FileName), ".ExternalAnnotations.xml"));

            var document = new XDocument();
            var root = new XElement("assembly", new XAttribute("name", ModuleDefinition.Assembly.Name.Name));

            root.Add(elements.OrderBy(el => el?.Attribute("name")?.Value.Substring(2)));

            document.Add(root);

            if (ContentEquals(targetName, document))
                return;

            document.Save(targetName);
        }

        private static bool ContentEquals(string targetName, XNode document)
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
            private readonly ICollection<string> _allAttributes;
            private readonly IList<XElement> _root;
            private readonly ModuleDefinition _moduleDefinition;

            public Executor(ModuleDefinition moduleDefinition, AssemblyDefinition jetbrainsAnnotations)
            {
                _moduleDefinition = moduleDefinition;
                _root = new List<XElement>();
                _allAttributes = new HashSet<string>(GetAllAttributes(jetbrainsAnnotations));
            }

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
                var content = new XElement("member", new XAttribute("name", id));

                foreach (var attribute in attributes)
                {
                    content.Add(new XElement("attribute", new XAttribute("ctor", DocCommentId.GetDocCommentId(attribute.Constructor.Resolve()))));
                }

                _root.Add(content);
            }

            private void ProcessMethodAttributes(MethodDefinition method)
            {
                var attributes = TrimAttributes(method);
                var isPublicVisible = method.IsPublicVisible();

                TrimAttributes(method.MethodReturnType);

                var id = DocCommentId.GetDocCommentId(method);
                var content = new XElement("member", new XAttribute("name", id));

                foreach (var attribute in attributes)
                {
                    AddAttributeDocumentation(content, attribute);
                }

                ProcessParameterAttributes(method, content, isPublicVisible);

                foreach (var parameter in method.GenericParameters)
                {
                    TrimAttributes(parameter);
                }

                if (isPublicVisible && content.HasElements)
                {
                    _root.Add(content);
                }
            }

            private void ProcessParameterAttributes(MethodDefinition method, XElement parent, bool isPublicVisible)
            {
                foreach (var parameter in method.Parameters)
                {
                    var parameterAttributes = TrimAttributes(parameter);

                    if (!isPublicVisible)
                        continue;

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

            private void AddAttributeDocumentation(XElement content, CustomAttribute attribute)
            {
                var constructor = attribute.Constructor.Resolve();

                var id = DocCommentId.GetDocCommentId(constructor);

                var element = new XElement("attribute", new XAttribute("ctor", id));

                foreach (var argument in attribute.ConstructorArguments)
                {
                    element.Add(new XElement("argument", argument.Value?.ToString()));
                }

                content.Add(element);
            }

            private IList<CustomAttribute> TrimAttributes(ICustomAttributeProvider attributeProvider)
            {
                var customAttributes = attributeProvider.CustomAttributes;

                var attributes = customAttributes
                    .Where(attr => _allAttributes.Contains(attr.AttributeType.Name))
                    .ToArray();

                foreach (var attribute in attributes)
                {
                    customAttributes.Remove(attribute);
                }

                return attributes;
            }

            private static IEnumerable<string> GetAllAttributes(AssemblyDefinition assemblyDefinition)
            {
                return assemblyDefinition.MainModule.Types
                    .Select(type => type.Name)
                    .Where(attributeName => attributeName.EndsWith("Attribute"));
            }
        }
    }
}