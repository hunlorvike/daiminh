namespace core.Entities.Shared;

public interface ISeeder<out T> where T : BaseEntity
{
    IEnumerable<T> DataSeeder();
}