using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace shared.Extensions;

/// <summary>
/// Provides extension methods for working with enums, particularly for generating <see cref="SelectListItem"/>
/// objects for use in dropdown lists in ASP.NET Core MVC.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts an enum to a list of <see cref="SelectListItem"/>, suitable for use in a dropdown list.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="addEmptyOption">Whether to add an empty option at the beginning of the list.</param>
    /// <param name="emptyText">The text to display for the empty option (defaults to "-- Select --").</param>
    /// <returns>A list of <see cref="SelectListItem"/> objects.</returns>
    /// <exception cref="ArgumentException">Thrown if <typeparamref name="TEnum"/> is not an enum type.</exception>
    public static List<SelectListItem> ToSelectList<TEnum>(bool addEmptyOption = false,
        string emptyText = "-- Select --")
        where TEnum : struct, Enum
    {
        var items = new List<SelectListItem>();

        if (addEmptyOption)
        {
            items.Add(new SelectListItem
            {
                Value = "",
                Text = emptyText
            });
        }

        items.AddRange(GetEnumSelectItems<TEnum>());
        return items;
    }

    /// <summary>
    /// Converts an enum to a list of <see cref="SelectListItem"/>, with a specified value pre-selected.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="selectedValue">The enum value to select.</param>
    /// <param name="addEmptyOption">Whether to add an empty option at the beginning of the list.</param>
    /// <param name="emptyText">The text to display for the empty option (defaults to "-- Select --").</param>
    /// <returns>A list of <see cref="SelectListItem"/> objects.</returns>
    /// <exception cref="ArgumentException">Thrown if <typeparamref name="TEnum"/> is not an enum type.</exception>
    public static List<SelectListItem> ToSelectList<TEnum>(TEnum? selectedValue, bool addEmptyOption = false,
        string emptyText = "-- Select --")
        where TEnum : struct, Enum
    {
        var items = new List<SelectListItem>();

        if (addEmptyOption)
        {
            items.Add(new SelectListItem
            {
                Value = "",
                Text = emptyText,
                Selected = !selectedValue.HasValue // Select the empty option if no value is selected
            });
        }

        items.AddRange(GetEnumSelectItems(selectedValue));
        return items;
    }

    /// <summary>
    /// Generates a sequence of <see cref="SelectListItem"/> objects from an enum.  This is a helper method
    /// used by the <see cref="ToSelectList{TEnum}(bool, string)"/> and
    /// <see cref="ToSelectList{TEnum}(TEnum?, bool, string)"/> methods.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="selectedValue">The currently selected enum value (optional).</param>
    /// <returns>An enumerable collection of <see cref="SelectListItem"/> objects.</returns>
    private static IEnumerable<SelectListItem> GetEnumSelectItems<TEnum>(TEnum? selectedValue = null)
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(value => new SelectListItem
            {
                Value = value.ToString("d"), // Use numeric value for Value. More robust than string.
                Text = GetEnumDisplayName(value),
                Selected = EqualityComparer<TEnum>.Default.Equals(value, selectedValue ?? default)
            });
    }

    /// <summary>
    /// Gets the display name for an enum value, considering <see cref="DisplayAttribute"/>,
    /// <see cref="DisplayNameAttribute"/>, and <see cref="DescriptionAttribute"/>, in that order.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name for the enum value.</returns>
    private static string GetEnumDisplayName<TEnum>(TEnum value) where TEnum : Enum
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        if (memberInfo == null) return value.ToString(); // Fallback to string representation

        var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            return displayAttribute.GetName() ?? value.ToString(); // Use GetName() and fallback
        }

        var displayNameAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttribute != null)
        {
            return displayNameAttribute.DisplayName;
        }

        var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
        if (descriptionAttribute != null)
        {
            return descriptionAttribute.Description;
        }

        return value.ToString(); // Fallback to string representation
    }
}