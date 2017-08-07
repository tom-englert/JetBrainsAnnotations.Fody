using System;
using System.Reflection;

using Mono.Cecil;

namespace Tests
{
    using JetBrains.Annotations;

    public class MockAssemblyResolver : IAssemblyResolver
    {
        [CanBeNull]
        public AssemblyDefinition Resolve([NotNull] AssemblyNameReference name)
        {
            // ReSharper disable once PossibleNullReferenceException
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
}