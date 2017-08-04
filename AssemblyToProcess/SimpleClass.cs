using JetBrains.Annotations;

[assembly: PublicAPIAttribute]
[module: PublicAPIAttribute]

[UsedImplicitly]
public class SimpleClass
{
    [PublicAPIAttribute]
    int field;
    
    [PublicAPIAttribute]
    public int Property { get; set; }

    [PublicAPIAttribute]
    [return: PublicAPIAttribute]
    public void Method<[PublicAPIAttribute]T>()
    {
        
    }

    [PublicAPIAttribute]
    public SimpleClass()
    {
        
    }

    [NotNull]
    public string NotNullMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
    {
        return "";
    }

    [NotNull]
    [StringFormatMethod("format")]
    public string StringFormat([NotNull] string format, [NotNull] params object[] args)
    {
        return "";
    }

    [ContractAnnotation("param1:null => null")]
    public string ContractMethod(string param1, string param2)
    {
        return "";
    }

    public string MethodWithoutAnyAttribute(string arg)
    {
        return "";
    }
}