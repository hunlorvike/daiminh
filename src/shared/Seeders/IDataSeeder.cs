namespace shared.Seeders;

public interface IDataSeeder
{
    Task SeedAsync();
    int Order { get; }
}