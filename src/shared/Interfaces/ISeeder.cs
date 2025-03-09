using shared.Models;

namespace domain.Entities.Shared;

public interface ISeeder<out T> where T : BaseEntity
{
    IEnumerable<T> DataSeeder();
}