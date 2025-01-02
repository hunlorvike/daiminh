using System.Text.Json;

namespace core.Common.Extensions;

public static class TypedValueExtensions
{
    public static T? GetTypedValue<T>(this string value, string type)
    {
        if (string.IsNullOrEmpty(value))
            return default;

        return type.ToLower() switch
        {
            "json" => JsonSerializer.Deserialize<T>(value),
            "int" => (T)Convert.ChangeType(value, typeof(T)),
            "bool" => (T)Convert.ChangeType(bool.Parse(value), typeof(T)),
            "datetime" => (T)Convert.ChangeType(DateTime.Parse(value), typeof(T)),
            _ => (T)Convert.ChangeType(value, typeof(T))
        };
    }

    public static string SetTypedValue<T>(this T value, string type)
    {
        if (value == null)
            return string.Empty;

        return type.ToLower() switch
        {
            "json" => JsonSerializer.Serialize(value),
            _ => value.ToString() ?? string.Empty
        };
    }
}