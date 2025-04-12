using shared.Enums;

namespace web.Areas.Admin.ViewModels.Setting;

public class SettingViewModel
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public FieldType Type { get; set; }
    public string? Description { get; set; }
    public string? DefaultValue { get; set; }
    public string? Value { get; set; }
    public bool IsActive { get; set; } = true;

    // Helper to determine HTML input type based on the enum
    public string InputType => GetHtmlInputType(this.Type);

    private static string GetHtmlInputType(FieldType settingType)
    {
        return settingType switch
        {
            FieldType.Text => "text",
            FieldType.Number => "number",
            FieldType.Boolean => "checkbox",
            FieldType.Email => "email",
            FieldType.Url => "url",
            FieldType.Color => "color",
            FieldType.Date => "date",
            // FieldType.Time => "time", // HTML5 time input support can be inconsistent
            // FieldType.DateTime => "datetime-local", // HTML5 datetime-local input
            // FieldType.Password => "password", // Use text for now unless special handling is needed
            FieldType.Phone => "tel",
            FieldType.TextArea => "textarea",
            FieldType.Html => "textarea",
            FieldType.Image => "image",
            FieldType.File => "file",
            // Add cases for Select, MultiSelect if needed (require extra data)
            _ => "text", // Default to text
        };
    }
}
