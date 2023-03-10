using System.Text.RegularExpressions;

namespace Framework.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class RegexAttribute : AbstractValidateAttribute
{
    private readonly string _regex;

    public RegexAttribute(string regex)
    {
        _regex = regex;
    }
    public override bool IsValidate(object value)
    {
        return value != null && Regex.IsMatch(value.ToString(), $@"{_regex}");
    }
}