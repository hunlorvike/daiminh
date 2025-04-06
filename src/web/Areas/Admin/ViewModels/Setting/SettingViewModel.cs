namespace web.Areas.Admin.ViewModels.Setting;

public class SettingViewModel
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "General", "Email", "Social Media", "Payment", "SEO", "Contact", "Company Info", "Theme", "Analytics", "Security", "Cache", "API", "Custom"
    public string Type { get; set; } = string.Empty; // e.g., "Text", "Number", "Boolean", "JSON", "HTML", "Image", "File", "Color", "Date", "Time", "DateTime", "Email", "URL", "Phone", "Address", "Currency", "Percentage", "Select", "MultiSelect", "Radio", "Checkbox", "TextArea", "RichText", "Code", "Password", "Secret", "Other"
    public string? Description { get; set; } // Description of the setting  
    public string? DefaultValue { get; set; } // Default value for the setting
    public string? Value { get; set; } // Current value of the setting
    public bool IsActive { get; set; } = true; // Whether the setting is active

    public string InputType => GetHtmlInputType(Type);

    private static string GetHtmlInputType(string settingType)
    {
        return settingType.ToLowerInvariant() switch
        {
            "text" => "text",
            "number" => "number",
            "boolean" => "checkbox", // Special handling needed in view/controller
            "email" => "email",
            "url" => "url",
            "color" => "color",
            "date" => "date",
            "time" => "time",
            "datetime" => "datetime-local",
            "password" => "password",
            "phone" => "tel",
            "textarea" => "textarea", // Custom value, handled in view
            "richtext" => "richtext", // Custom value, handled in view
            "json" => "textarea",     // Custom value, handled in view
            "code" => "textarea",      // Custom value, handled in view
            _ => "text",              // Default to text
        };
    }
}
