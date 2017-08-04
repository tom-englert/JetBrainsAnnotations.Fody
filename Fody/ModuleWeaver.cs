using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;

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

        var root = new Executor(ModuleDefinition, jetbrainsAnnotations).Execute();

        if (!root.HasElements)
            return;

        Save(root);
    }

    private void Save(XElement root)
    {
        var targetName = Path.Combine(ProjectDirectoryPath, Path.ChangeExtension(Path.GetFileName(ModuleDefinition.FileName), ".ExternalAnnotations.xml"));

        var document = new XDocument();
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
        private readonly XElement _root;
        private readonly ModuleDefinition _moduleDefinition;

        public Executor(ModuleDefinition moduleDefinition, AssemblyDefinition jetbrainsAnnotations)
        {
            _moduleDefinition = moduleDefinition;
            _root = new XElement("assembly", new XAttribute("name", moduleDefinition.Assembly.Name.Name));
            _allAttributes = new HashSet<string>(GetAllAttributes(jetbrainsAnnotations));
        }

        public XElement Execute()
        {
            TrimAttributes(_moduleDefinition);
            TrimAttributes(_moduleDefinition.Assembly);

            foreach (var type in _moduleDefinition.GetTypes())
            {
                ProcessAttributes(type);

                foreach (var parameter in type.GenericParameters)
                {
                    TrimAttributes(parameter);
                }

                foreach (var field in type.Fields)
                {
                    TrimAttributes(field);
                }

                foreach (var anEvent in type.Events)
                {
                    ProcessAttributes(anEvent);
                }

                foreach (var property in type.Properties)
                {
                    ProcessAttributes(property);
                }

                foreach (var method in type.Methods)
                {
                    ProcessMethodAttributes(method);
                }
            }

            return _root;
        }

        private void ProcessAttributes(IMemberDefinition member)
        {
            var attributes = TrimAttributes(member);
            if (!attributes.Any())
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

            var id = DocCommentId.GetDocCommentId(method);
            var content = new XElement("member", new XAttribute("name", id));

            foreach (var attribute in attributes)
            {
                AddAttributeDocumentation(content, attribute);
            }

            TrimAttributes(method.MethodReturnType);

            ProcessParameterAttributes(method, content);

            foreach (var parameter in method.GenericParameters)
            {
                TrimAttributes(parameter);
            }

            if (content.HasElements)
            {
                _root.Add(content);
            }
        }

        private void ProcessParameterAttributes(MethodDefinition method, XElement parent)
        {
            foreach (var parameter in method.Parameters)
            {
                var parameterAttributes = TrimAttributes(parameter);

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