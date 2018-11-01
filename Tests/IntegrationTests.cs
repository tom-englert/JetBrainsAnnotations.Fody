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
        private readonly Assembly _assembly;

        public IntegrationTests()
        {
            var thisFolder = Path.GetDirectoryName(new Uri(GetType().Assembly.CodeBase).LocalPath);

            _assembly = Assembly.LoadFrom(Path.Combine(thisFolder, "AssemblyToProcess.dll"));
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
            var annotations = XDocument.Load(Path.ChangeExtension(_assembly.Location, ".ExternalAnnotations.xml")).ToString();

            Assert.Equal(Resources.ExpectedAnnotations, annotations);
        }

        [Fact]
        public void IsDocumentationProperlyDecorated()
        {
            var _documentation = XDocument.Load(Path.ChangeExtension(_assembly.Location, ".xml")).ToString();

            Assert.Equal(Resources.ExpectedDocumentation, _documentation);
        }
    }
}