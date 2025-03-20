using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace shared.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a string into a URL-friendly slug.
    /// This method removes diacritic marks (e.g., accents in Vietnamese), 
    /// converts the text to lowercase, removes special characters, 
    /// and replaces spaces with hyphens ("-").
    /// </summary>
    /// <param name="text">The input string to be converted into a slug.</param>
    /// <returns>
    /// A URL-friendly slug version of the input string. 
    /// Returns an empty string if the input is null or empty.
    /// </returns>
    /// <example>
    /// <code>
    /// string slug = "Thư mục Hình ảnh đẹp 2024".ToUrlSlug();
    /// Console.WriteLine(slug); // Output: "thu-muc-hinh-anh-dep-2024"
    /// </code>
    /// </example>
    public static string ToUrlSlug(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        text = text.Normalize(NormalizationForm.FormD);

        var stringBuilder = new StringBuilder();
        foreach (var c in text)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        string normalizedText = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        normalizedText = normalizedText.ToLowerInvariant();

        normalizedText = Regex.Replace(normalizedText, @"[^a-z0-9\s-]", "");

        normalizedText = Regex.Replace(normalizedText, @"\s+", "-").Trim('-');

        return normalizedText;
    }
}