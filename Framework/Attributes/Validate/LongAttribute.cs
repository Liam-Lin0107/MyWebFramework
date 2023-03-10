namespace Framework.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class LongAttribute : AbstractValidateAttribute
{
    private readonly long _Min;
    private readonly long _Max;
    public LongAttribute(long min, long max)
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