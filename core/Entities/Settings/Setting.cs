using core.Entities.Shared;

namespace core.Entities.Settings;

public class Setting : BaseEntity, IActivatable
{
    public string Key { get; set; } = null!;
    public string? Value { get; set; }
    public string? Group { get; set; }
    public string? SubGroup { get; set; }
    public string Type { get; set; } = "string";
    public int? DisplayOrder { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class EmailTemplate : BaseEntity, IActivatable
{
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string? Description { get; set; }
    public string Type { get; set; } = null!;
    public string? Variables { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? UpdatedAt { get; set; }
}