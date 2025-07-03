using System;

namespace SneakersShop.Application.UseCases.DTO;

public class ProductsDto : BaseDto
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string Code { get; set; }
    public double AvgRating { get; set; }
    public int ReviewCount { get; set; }
    public decimal OldPrice { get; set; }
    public decimal? NewPrice { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public string Image { get; set; }
}
