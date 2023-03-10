using System.Reflection;
using Framework.Attributes;
using Framework.Model;

namespace Framework.Attributes;

public static class AttributeHelper
{
    public static string GetColumnName(this PropertyInfo prop)
    {
        if (prop.IsDefined(typeof(ColumnAttribute), true))
        {
            var attribute = prop.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
            return attribute.ColumnName;
        }
        return prop.Name;
    }

    public static void Validate<T>(this T model) where T : BaseModel
    {
        Type type = model.GetType();
        foreach (var prop in type.GetProperties())
        {
            if (prop.IsDefined(typeof(AbstractValidateAttribute), true))
            {
                var attributes = prop.GetCustomAttributes(typeof(AbstractValidateAttribute), true);
                foreach (AbstractValidateAttribute attribute in attributes)
                {
                    if (!attribute.IsValidate(prop.GetValue(model)))
                    {
                        throw new Exception($"Incorrected data: The {prop.Name} is {prop.GetValue(model)}");
                    }
                }
            }
        }
    }

}
