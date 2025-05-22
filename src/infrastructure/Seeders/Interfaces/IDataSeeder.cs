namespace infrastructure.Seeders.Interfaces;

public interface IDataSeeder
{
    Task SeedAsync();
    int Order { get; }
}