using System.Reflection;

[assembly: AssemblyTitle(Product.Name)]
[assembly: AssemblyProduct(Product.Name)]
[assembly: AssemblyVersion(Product.Version)]
[assembly: AssemblyFileVersion(Product.Version)]

static class Product
{
    public const string Name = "JetBrainsAnnotations.Fody";
    public const string Version = "1.1.5";
}