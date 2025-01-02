namespace core.Entities.Shared;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; }
}

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public abstract class ActivatableEntity : AuditableEntity, IActivatable
{
    public bool IsActive { get; set; } = true;
}