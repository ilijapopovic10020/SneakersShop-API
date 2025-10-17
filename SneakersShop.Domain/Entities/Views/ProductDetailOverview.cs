using System;

namespace SneakersShop.Domain.Entities.Views;

public class ProductDetailOverview
{
    public int ProductColorId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal OldPrice { get; set; }
    public decimal? NewPrice { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal AvgRating { get; set; }
    public int ReviewCount { get; set; }
    public string? FilePath { get; set; }
}
