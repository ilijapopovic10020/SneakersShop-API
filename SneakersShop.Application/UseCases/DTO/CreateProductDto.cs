using System;

namespace SneakersShop.Application.UseCases.DTO;

public class CreateProductDto
{
    public string Name { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }

    public IEnumerable<CreateProductVariantDto> ProductVariants { get; set; }
}

public class CreateProductVariantDto
{
    public int ColorId { get; set; }
    public string Code { get; set; }
    public int Quantity { get; set; }

    public IEnumerable<CreateProductVaraintImageDto> VariantImages { get; set; }
    public IEnumerable<int> SizeIds { get; set; }
}

public class CreateProductVaraintImageDto
{
    public string ImagePath { get; set; }
    public string ImageName { get; set; }
}

