namespace Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using JetBrains.Annotations;

    using Tests.Properties;

    using Xunit;

    public class IntegrationTests
    {
        [NotNull]
        private readonly string _targetAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AssemblyToProcess.dll");
        [NotNull]
        private readonly Assembly _assembly;

        public IntegrationTests()
        {
            _assembly = Assembly.LoadFrom(_targetAssemblyPath);
        }

        [Fact]
        public void CanCreateClass()
        {
            var type = _assembly.GetType("AssemblyToProcess.SimpleClass");
            // ReSharper disable once AssignNullToNotNullAttribute
            Activator.CreateInstance(type);
        }

        [Fact]
        public void ReferenceIsRemoved()
        {
            // ReSharper disable once PossibleNullReferenceException
            Assert.DoesNotContain(_assembly.GetReferencedAssemblies(), x => x.Name == "JetBrains.Annotations");
        }


        [Fact]
        public void AreExternalAnnotationsCorrect()
        {
            var annotations = XDocument.Load(Path.ChangeExtension(_targetAssemblyPath, ".ExternalAnnotations.xml")).ToString();

            Assert.Equal(Resources.ExpectedAnnotations, annotations);
        }

        [Fact]
        public void IsDocumentationProperlyDecorated()
        {
            var documentation = XDocument.Load(Path.ChangeExtension(_targetAssemblyPath, ".xml")).ToString();

            Assert.Equal(XDocument.Parse(Resources.ExpectedDocumentation).ToString(), documentation);
        }
    }
}