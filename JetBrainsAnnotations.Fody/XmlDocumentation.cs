namespace JetBrainsAnnotations.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using JetBrains.Annotations;

    internal static class XmlDocumentation
    {
        public static void Decorate([NotNull] string documentationFileName, [NotNull, ItemNotNull] IEnumerable<XElement> annotationMembers)
        {
            var doc = XDocument.Load(documentationFileName, LoadOptions.PreserveWhitespace);

            var docMembers = doc.Descendants("member").ToDictionary(node => node.Attribute("name").Value);

            foreach (var annotationMember in annotationMembers)
            {
                var memberName = annotationMember.Attribute("name")?.Value;
                if (string.IsNullOrEmpty(memberName))
                    continue;

                if (!docMembers.TryGetValue(memberName, out var docMember))
                    continue;

                var elementDocumentation = GetAttributeDocumentation(annotationMember);

                if (!string.IsNullOrEmpty(elementDocumentation))
                {
                    var summary = docMember.Elements("summary").FirstOrDefault();
                    if (summary != null)
                    {
                        AddAttributeDocumentation(summary, elementDocumentation);
                    }
                }

                var docMemberParameters = docMember.Elements("param").ToDictionary(node => node.Attribute("name").Value);

                foreach (var annotationMemberParameter in annotationMember.Elements("parameter"))
                {
                    var attributeDocumentation = GetAttributeDocumentation(annotationMemberParameter);

                    if (string.IsNullOrEmpty(attributeDocumentation))
                        continue;

                    if (!docMemberParameters.TryGetValue(annotationMemberParameter.Attribute("name").Value, out var docMemberParameter))
                        continue;

                    AddAttributeDocumentation(docMemberParameter, attributeDocumentation);
                }
            }

            doc.Save(documentationFileName, SaveOptions.DisableFormatting);
        }

        private static void AddAttributeDocumentation([NotNull] XElement element, [NotNull] string attributeDocumentation)
        {
            if (element.Value.EndsWith(attributeDocumentation))
                return;

            var codeElement = new XElement("c");
            codeElement.Add(new XText(attributeDocumentation));

            element.Add(new XElement("p"), codeElement);
        }

        private static string GetAttributeDocumentation([NotNull] XContainer element)
        {
            var documentation = string.Join(", ", EnumerateAttributes(element));
            if (string.IsNullOrEmpty(documentation))
                return null;

            return "[" + documentation + "]";
        }

        [NotNull]
        private static IEnumerable<string> EnumerateAttributes([NotNull] XContainer element)
        {
            return element.Elements("attribute")
                .Select(node => node.Attribute("ctor")?.Value)
                .Select(ExtractAttributeName)
                .Where(name => name != null);
        }

        private static string ExtractAttributeName(string arg)
        {
            const string prefix = "M:JetBrains.Annotations.";
            const string suffix = "Attribute.#ctor";

            if ((arg != null) && arg.StartsWith(prefix) && arg.EndsWith(suffix))
            {
                return arg.Substring(prefix.Length, arg.Length - prefix.Length - suffix.Length);
            }

            return null;
        }
    }
}
