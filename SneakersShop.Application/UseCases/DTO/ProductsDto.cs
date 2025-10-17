using System;

namespace SneakersShop.Application.UseCases.DTO;

public class ProductsDto
{
    public int ProductColorId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal OldPrice { get; set; }
    public int BrandId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double AvgRating { get; set; }
    public int ReviewCount { get; set; }
    public decimal? NewPrice { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public int SoldQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
}
