    using System;

    namespace SneakersShop.Application.UseCases.DTO;

    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public string Code { get; set; }
        public decimal OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public IEnumerable<string> Images { get; set; }
        public bool IsFavorite { get; set; } = false;
        public IEnumerable<ProductVariantDto> Variants { get; set; }
        public IEnumerable<ProductSizeDto> Sizes { get; set; }
    }

    public class ProductVariantDto : BaseDto
    {
        public string Image { get; set; }
    }

    public class ProductSizeDto : BaseDto
    {
        public decimal Number { get; set; }
        public string Detail { get; set; }
        public int Quantity { get; set; }
    }
