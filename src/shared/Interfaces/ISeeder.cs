using shared.Models;

namespace shared.Interfaces;

public interface ISeeder<out T> where T : BaseEntity
{
    IEnumerable<T> DataSeeder();
}