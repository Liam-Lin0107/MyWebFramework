namespace Framework.Attributes;

public abstract class AbstractValidateAttribute : Attribute
{
    public abstract bool IsValidate(object value);
}