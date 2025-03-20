using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace shared.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        Type type = enumValue.GetType();
        MemberInfo[] memberInfo = type.GetMember(enumValue.ToString());
        DisplayAttribute? attribute = memberInfo.FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ?? enumValue.ToString();
    }
}
