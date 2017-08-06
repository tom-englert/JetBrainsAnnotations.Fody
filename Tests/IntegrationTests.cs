using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using JetBrainsAnnotations.Fody;

using Mono.Cecil;

using NUnit.Framework;

using Tests.Properties;

namespace Tests
{
    [TestFixture]
    public class IntegrationTests
    {
#if (!DEBUG)
        private const string Configuration = "Release";
#else
        private const string Configuration = "Debug";
#endif
        private readonly Assembly _assembly;
        private readonly string _beforeAssemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, $@"..\..\..\AssemblyToProcess\bin\{Configuration}\AssemblyToProcess.dll"));
        private readonly string _afterAssemblyPath;
        private readonly string _annotations;

        public IntegrationTests()
        {
            _afterAssemblyPath = _beforeAssemblyPath.Replace(".dll", "2.dll");

            using (var moduleDefinition = ModuleDefinition.ReadModule(_beforeAssemblyPath))
            {
                var projectDirectoryPath = Path.GetDirectoryName(_beforeAssemblyPath);
                var targetName = Path.ChangeExtension(_beforeAssemblyPath, ".ExternalAnnotations.xml");

                if (File.Exists(targetName))
                    File.Delete(targetName);

                var weavingTask = new ModuleWeaver
                {
                    ModuleDefinition = moduleDefinition,
                    AssemblyResolver = new MockAssemblyResolver(),
                    ProjectDirectoryPath = projectDirectoryPath
                };

                weavingTask.Execute();

                _annotations = XDocument.Load(targetName).ToString();

                moduleDefinition.Write(_afterAssemblyPath);
            }

            _assembly = Assembly.LoadFile(_afterAssemblyPath);
        }

        [Test]
        public void CanCreateClass()
        {
            var type = _assembly.GetType("AssemblyToProcess.SimpleClass");
            Activator.CreateInstance(type);
        }

        [Test]
        public void ReferenceIsRemoved()
        {
            Assert.IsFalse(_assembly.GetReferencedAssemblies().Any(x => x.Name == "JetBrains.Annotations"));
        }


        [Test]
        public void AreExternalAnnotationsCorrect()
        {
            Assert.AreEqual(Resources.ExpectedAnnotations, _annotations);
        }

#if(DEBUG)
        [Test]
        public void PeVerify()
        {
            Verifier.Verify(_beforeAssemblyPath, _afterAssemblyPath);
        }
#endif

    }
}