namespace shared.Helpers;

public static class SlugHelper
{
    private static readonly Slugify.SlugHelper _helper = new();

    public static string Generate(string input)
    {
        return _helper.GenerateSlug(input);
    }
}
