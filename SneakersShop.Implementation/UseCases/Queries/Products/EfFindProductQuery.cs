using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Products;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Products
{
    public class EfFindProductQuery(SneakersShopDbContext context, IApplicationUser user)
        : EfUseCase(context, user),
            IFindProductQuery
    {
        public int Id => 6;

        public string Name => "Find Product";

        public string Description => "";

        public ProductDto Execute(int search)
        {
            var productColor =
                Context
                    .ProductColors.Include(pc => pc.Product)
                    .ThenInclude(p => p.Brand)
                    .Include(pc => pc.Product)
                    .ThenInclude(p => p.Category)
                    .Include(pc => pc.Color)
                    .Include(pc => pc.ProductSizes)
                    .ThenInclude(ps => ps.Size)
                    .Include(pc => pc.ProductImages)
                    .ThenInclude(pi => pi.Image) // tvoja tabela Files
                    .Include(pc => pc.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                    .Include(pc => pc.Product.Reviews)
                    .Include(pc => pc.Favorites)
                    .FirstOrDefault(pc => pc.Id == search)
                ?? throw new EntityNotFoundException(search, nameof(Product));

            var product = productColor.Product;

            // Aktivni popust
            var productDiscount = productColor
                .ProductDiscounts.Where(pd =>
                    pd.Discount.IsActive
                    && (!pd.Discount.StartDate.HasValue || pd.Discount.StartDate <= DateTime.UtcNow)
                    && (!pd.Discount.EndDate.HasValue || pd.Discount.EndDate >= DateTime.UtcNow)
                )
                .Select(pd => pd.Discount)
                .FirstOrDefault();

            decimal? newPrice = null;
            string? discountType = null;
            decimal? discountValue = null;
            decimal oldPrice = product.Price;

            if (productDiscount != null)
            {
                newPrice = Math.Round(product.Price * (1 - productDiscount.Percentage / 100), 2);
                discountType = productDiscount.Name.ToLower().Contains("popusti")
                    ? "Popust"
                    : productDiscount.Name;
                discountValue = productDiscount.Percentage;
            }

            // Kreiranje DTO
            var productDto = new ProductDto
            {
                Id = productColor.Id,
                Name = product.Name,
                Brand = product.Brand?.Name ?? string.Empty,
                Category = product.Category?.Name ?? string.Empty,
                Color = productColor.Color?.Name ?? string.Empty,
                Code = productColor.Code ?? string.Empty,
                AvgRating = product.Reviews.Any()
                    ? Math.Round((decimal)product.Reviews.Average(r => r.Rating), 1)
                    : 0,
                ReviewCount = product.Reviews.Count,
                OldPrice = oldPrice,
                NewPrice = newPrice,
                DiscountType = discountType,
                DiscountValue = discountValue,
                Images = productColor
                    .ProductImages.Where(pi => pi.Image != null)
                    .Select(pi =>
                        $"/images/products/{Path.GetFileName(pi.Image.Path ?? string.Empty)}"
                    )
                    .ToList(),
                Sizes = productColor
                    .ProductSizes.Where(ps => ps.Size != null)
                    .Select(ps => new ProductSizeDto
                    {
                        Id = ps.Size.Id,
                        Number = ps.Size.Number,
                        Detail = ps.Size.Detail,
                        Quantity = ps.Quantity,
                    })
                    .ToList(),
                Variants = Context
                    .ProductColors.Where(pc => pc.ProductId == product.Id)
                    .Select(pc => new ProductVariantDto
                    {
                        Id = pc.Id,
                        Image =
                            pc.ProductImages.Where(pi =>
                                    pi.Image != null
                                    && pi.Image.Path != null
                                    && pi.Image.Path.Contains("thumb")
                                )
                                .Select(pi => "/images/products/" + Path.GetFileName(pi.Image.Path))
                                .FirstOrDefault() ?? string.Empty,
                    })
                    .ToList(),
                IsFavorite = Context.Favorites.Any(f =>
                    f.UserId == User.Id && f.ProductColorId == productColor.Id
                ),
            };

            return productDto;
        }
    }
}
