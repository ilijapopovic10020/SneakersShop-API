using System;

namespace SneakersShop.Application.UseCases.DTO.Searches;

public abstract class PagedSearchKw : BasePagedSearch
{
    public string? Keyword { get; set; }
}
