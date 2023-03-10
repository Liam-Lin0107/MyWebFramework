namespace Framework.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class LengAttribute : AbstractValidateAttribute
{
    private readonly int _Min;
    private readonly int _Max;
    public LengAttribute(int min, int max)
    {
        _Min = min;
        _Max = max;
    }
    public override bool IsValidate(object value)
    {
        if (!string.IsNullOrWhiteSpace(value.ToString()))
        {
            int length = value.ToString().Length;
            return length >= _Min && length <= _Max;
        }
        return false;
    }
}