using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;

public class ModuleWeaver
{
    List<string> allAttributes = new List<string>();
    public string IncludeNamespaces { get; set; }
    public string ExcludeNamespaces { get; set; }
    public Action<string> LogInfo { get; set; }
    public Action<string> LogWarning { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public IAssemblyResolver AssemblyResolver { get; set; }

    public ModuleWeaver()
    {
        LogInfo = s => { };
        LogWarning = s => { };
    }

    public void Execute()
    {
        var assemblyNameReference = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name.StartsWith("JetBrains.Annotations"));
        if (assemblyNameReference == null)
        {
            LogWarning("Reference to JetBrains.Annotations not found.");
            return;
        }
        ModuleDefinition.AssemblyReferences.Remove(assemblyNameReference);
        var assemblyDefinition = AssemblyResolver.Resolve(assemblyNameReference);

        GetAttributes(assemblyDefinition);

        TrimAttributes(ModuleDefinition.Assembly.CustomAttributes);
        TrimAttributes(ModuleDefinition.CustomAttributes);
        foreach (var type in ModuleDefinition.GetTypes())
        {
            TrimAttributes(type.CustomAttributes);

            foreach (var parameter in type.GenericParameters)
            {
                TrimAttributes(parameter.CustomAttributes);
            }
            foreach (var field in type.Fields)
            {
                TrimAttributes(field.CustomAttributes);
            }
            foreach (var anEvent in type.Events)
            {
                TrimAttributes(anEvent.CustomAttributes);
            }
            foreach (var property in type.Properties)
            {
                TrimAttributes(property.CustomAttributes);
            }
            foreach (var method in type.Methods)
            {
                TrimAttributes(method.CustomAttributes);
                foreach (var parameter in method.Parameters)
                {
                    TrimAttributes(parameter.CustomAttributes);
                }
                TrimAttributes(method.MethodReturnType.CustomAttributes);
                foreach (var parameter in method.GenericParameters)
                {
                    TrimAttributes(parameter.CustomAttributes);
                }
            }
        }
    }

    void TrimAttributes(Collection<CustomAttribute> customAttributes)
    {
        foreach (var customAttribute in customAttributes.ToList())
        {
            if (allAttributes.Contains(customAttribute.AttributeType.Name))
            {
                customAttributes.Remove(customAttribute);
            }
        }
    }


    void GetAttributes(AssemblyDefinition assemblyDefinition)
    {
        foreach (var type in assemblyDefinition.MainModule.Types)
        {
            var attributeName = type.Name;
            if (attributeName.EndsWith("Attribute"))
            {
                allAttributes.Add(attributeName);
            }
        }
    }
}