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
}