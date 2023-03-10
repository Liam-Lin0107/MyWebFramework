namespace Framework.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    private readonly string _name;
    public string ColumnName => _name;
    public ColumnAttribute(string name)
    {
        _name = name;
    }
}
