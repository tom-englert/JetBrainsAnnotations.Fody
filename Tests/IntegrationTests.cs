using System;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    Assembly assembly;

    public IntegrationTests()
    {
        var projectPath = @"AssemblyToProcess\AssemblyToProcess.csproj";
#if (!DEBUG)

            projectPath = projectPath.Replace("Debug", "Release");
#endif
        var weaverHelper = new WeaverHelper(projectPath);
        assembly = weaverHelper.Assembly;
    }

    [Test]
    public void Simple()
    {
        var type = assembly.GetType("SimpleClass");
        Activator.CreateInstance(type);
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assembly.CodeBase.Remove(0, 8));
    }
#endif

}