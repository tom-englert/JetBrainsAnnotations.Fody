using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    Assembly assembly;
    string beforeAssemblyPath;
    string afterAssemblyPath;
    string jetBrainsFilePath;
    string jetBrainsBackupFilePath;

    public IntegrationTests()
    {
        beforeAssemblyPath = Path.GetFullPath(@"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)

        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif

        BackupJetBrainsAssembly();

        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(afterAssemblyPath);
        var weavingTask = new ModuleWeaver
        {
            ModuleDefinition = moduleDefinition,
            AssemblyResolver = new MockAssemblyResolver()
        };

        weavingTask.Execute();
        moduleDefinition.Write(afterAssemblyPath);

        assembly = Assembly.LoadFile(afterAssemblyPath);
    }

    [Test]
    public void Simple()
    {
        var type = assembly.GetType("SimpleClass");
        Activator.CreateInstance(type);
    }

    [Test]
    public void JetBrainsAssemblyDeleted()
    {
        Assert.That(File.Exists(beforeAssemblyPath.Replace("AssemblyToProcess.dll", "JetBrains.Annotations.dll")), Is.False);
    }

    [TestFixtureTearDown]
    public void TearDown()
    {
        if (!File.Exists(jetBrainsFilePath) && File.Exists(jetBrainsBackupFilePath))
        {
            //Restore JetBrains assembly deleted during testing
            File.Copy(jetBrainsBackupFilePath, jetBrainsFilePath);
        }
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
#endif

    private void BackupJetBrainsAssembly()
    {
        jetBrainsFilePath = beforeAssemblyPath.Replace("AssemblyToProcess.dll", "JetBrains.Annotations.dll");
        jetBrainsBackupFilePath = jetBrainsFilePath.Replace(".dll", ".bak");

        if (File.Exists(jetBrainsBackupFilePath)) return;

        Assert.That(File.Exists(jetBrainsFilePath), Is.True);
        File.Copy(jetBrainsFilePath, jetBrainsBackupFilePath);
    }

}