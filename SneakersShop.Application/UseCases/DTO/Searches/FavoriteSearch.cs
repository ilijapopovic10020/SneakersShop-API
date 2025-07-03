using System;

namespace SneakersShop.Application.UseCases.DTO.Searches;

public class FavoriteSearch
{
    public int? PerPage { get; set; }
    public int? Page { get; set; }
    public string? Keyword { get; set; }
}
