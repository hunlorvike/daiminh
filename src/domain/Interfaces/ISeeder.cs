using shared.Models;

namespace domain.Interfaces;

public interface ISeeder<out T> where T : BaseEntity
{
    IEnumerable<T> DataSeeder();
}