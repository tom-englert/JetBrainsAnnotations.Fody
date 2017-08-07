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
    using System.Diagnostics;

    using JetBrains.Annotations;

    [TestFixture]
    public class IntegrationTests
    {
#if (!DEBUG)
        private const string Configuration = "Release";
#else
        private const string Configuration = "Debug";
#endif
        [NotNull]
        private readonly Assembly _assembly;
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        // ReSharper disable once PossibleNullReferenceException
        private readonly string _beforeAssemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, $@"..\..\..\AssemblyToProcess\bin\{Configuration}\AssemblyToProcess.dll"));
        [NotNull]
        private readonly string _afterAssemblyPath;
        [NotNull]
        private readonly string _annotations;

        public IntegrationTests()
        {
            _afterAssemblyPath = _beforeAssemblyPath.Replace(".dll", "2.dll");

            var assemblyResolver = new MockAssemblyResolver();

            using (var moduleDefinition = ModuleDefinition.ReadModule(_beforeAssemblyPath, new ReaderParameters { AssemblyResolver = assemblyResolver}))
            {
                var projectDirectoryPath = Path.GetDirectoryName(_beforeAssemblyPath);
                var targetName = Path.ChangeExtension(_beforeAssemblyPath, ".ExternalAnnotations.xml");

                if (File.Exists(targetName))
                    File.Delete(targetName);

                Debug.Assert(moduleDefinition != null, "moduleDefinition != null");
                Debug.Assert(projectDirectoryPath != null, "projectDirectoryPath != null");
                var weavingTask = new ModuleWeaver
                {
                    ModuleDefinition = moduleDefinition,
                    AssemblyResolver = assemblyResolver,
                    ProjectDirectoryPath = projectDirectoryPath
                };

                weavingTask.Execute();

                _annotations = XDocument.Load(targetName).ToString();

                moduleDefinition.Write(_afterAssemblyPath);
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            _assembly = Assembly.LoadFile(_afterAssemblyPath);
        }

        [Test]
        public void CanCreateClass()
        {
            var type = _assembly.GetType("AssemblyToProcess.SimpleClass");
            // ReSharper disable once AssignNullToNotNullAttribute
            Activator.CreateInstance(type);
        }

        [Test]
        public void ReferenceIsRemoved()
        {
            // ReSharper disable once PossibleNullReferenceException
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