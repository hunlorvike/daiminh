namespace core.Entities.Shared;

public interface IEntity
{
    Guid Id { get; set; }
}

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
}

public interface IActivatable
{
    bool IsActive { get; set; }
}