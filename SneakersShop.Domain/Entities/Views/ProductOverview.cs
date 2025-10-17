namespace SneakersShop.Domain.Entities.Views;

public class ProductOverview
{
    public int ProductColorId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal OldPrice { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public double AvgRating { get; set; }
    public int ReviewCount { get; set; }
    public decimal? NewPrice { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public int SoldQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
}
