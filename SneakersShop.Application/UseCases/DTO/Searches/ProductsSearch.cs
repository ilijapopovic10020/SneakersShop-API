using System;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Application.UseCases.DTO.Searches;

public class ProductsSearch : PagedSearchKw
{
    public int? CategoryId { get; set; }
    public List<int>? BrandId { get; set; }
    public List<string>? Color { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public Filter? Filter { get; set; }
}
