using System;
using System.Reflection;
using Mono.Cecil;

public class MockAssemblyResolver : IAssemblyResolver
{
    public AssemblyDefinition Resolve(AssemblyNameReference name)
    {
        var codeBase = Assembly.Load(name.FullName).CodeBase.Replace("file:///", "");
        return AssemblyDefinition.ReadAssembly(codeBase);
    }

    public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }
}