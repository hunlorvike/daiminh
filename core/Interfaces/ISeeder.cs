using core.Entities;

namespace core.Interfaces;

public interface ISeeder<out T> where T : BaseEntity
{
    IEnumerable<T> DataSeeder();
}