using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Common.Extensions;

public static class EnumExtensions
{
    public static List<SelectListItem> ToSelectList<TEnum>(bool addEmptyOption = false,
        string emptyText = "-- Select --")
        where TEnum : struct, Enum
    {
        var items = new List<SelectListItem>();

        if (addEmptyOption)
            items.Add(new SelectListItem
            {
                Value = "",
                Text = emptyText
            });

        items.AddRange(GetEnumSelectItems<TEnum>());
        return items;
    }

    public static List<SelectListItem> ToSelectList<TEnum>(TEnum? selectedValue, bool addEmptyOption = false,
        string emptyText = "-- Select --")
        where TEnum : struct, Enum
    {
        var items = new List<SelectListItem>();

        if (addEmptyOption)
            items.Add(new SelectListItem
            {
                Value = "",
                Text = emptyText,
                Selected = !selectedValue.HasValue
            });

        items.AddRange(GetEnumSelectItems(selectedValue));
        return items;
    }

    private static IEnumerable<SelectListItem> GetEnumSelectItems<TEnum>(TEnum? selectedValue = null)
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(value => new SelectListItem
            {
                Value = value.ToString("d"),
                Text = GetEnumDisplayName(value),
                Selected = EqualityComparer<TEnum>.Default.Equals(value, selectedValue ?? default)
            });
    }

    private static string GetEnumDisplayName<TEnum>(TEnum value) where TEnum : Enum
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        if (memberInfo == null) return value.ToString();

        var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
            return displayAttribute.GetName() ?? value.ToString();

        var displayNameAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttribute != null)
            return displayNameAttribute.DisplayName;

        var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
        if (descriptionAttribute != null)
            return descriptionAttribute.Description;

        return value.ToString();
    }
}