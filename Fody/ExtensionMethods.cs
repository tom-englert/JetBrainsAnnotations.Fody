namespace JetBrainsAnnotations.Fody
{
    using JetBrains.Annotations;

    using Mono.Cecil;

    internal static class ExtensionMethods
    {
        public static bool IsPublicVisible([CanBeNull] this TypeDefinition member)
        {
            return member == null || member.IsPublic || (member.IsNestedPublic && member.DeclaringType.IsPublicVisible());
        }

        public static bool IsPublicVisible([CanBeNull] this FieldDefinition member)
        {
            return member != null && (member.IsPublic || member.IsFamily) && member.DeclaringType.IsPublicVisible();
        }

        public static bool IsPublicVisible([CanBeNull] this MethodDefinition member)
        {
            return member != null && (member.IsPublic || member.IsFamily) && member.DeclaringType.IsPublicVisible();
        }

        public static bool IsPublicVisible([CanBeNull] this EventDefinition member)
        {
            return member != null && member.DeclaringType.IsPublicVisible() && (member.AddMethod.IsPublicVisible() || member.RemoveMethod.IsPublicVisible());
        }

        public static bool IsPublicVisible([CanBeNull] this PropertyDefinition member)
        {
            return member != null && member.DeclaringType.IsPublicVisible() && (member.SetMethod.IsPublicVisible() || member.GetMethod.IsPublicVisible());
        }
    }
}
