using System.ComponentModel.DataAnnotations;

namespace shared.Models;

public class PaginationSettings
{
    [Range(1, 100)]
    public int AdminDefaultPageSize { get; set; } = 15;
    [Range(1, 100)]
    public int ClientProductPageSize { get; set; } = 12;
    [Range(1, 100)]
    public int ClientArticlePageSize { get; set; } = 9;
    [Range(1, 100)]
    public int ClientSearchPageSize { get; set; } = 10;
}

