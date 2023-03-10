using System.Text.RegularExpressions;

namespace Framework.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class EmailAttribute : AbstractValidateAttribute
{
    public override bool IsValidate(object value)
    {
        return value != null && Regex.IsMatch(value.ToString(), @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
    }
}