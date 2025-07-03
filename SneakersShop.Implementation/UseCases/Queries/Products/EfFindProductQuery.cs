using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Products;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Products;

public class EfFindProductQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IFindProductQuery
{
    public int Id => 6;

    public string Name => "Find Product";

    public string Description => "";

    public ProductDto Execute(int search)
    {
        var productColor = Context.ProductColors.Include(pc => pc.Color)
                           .Include(pc => pc.Product)
                            .ThenInclude(p => p.Brand)
                           .Include(pc => pc.Product)
                            .ThenInclude(p => p.Category)
                           .Include(pc => pc.ProductImages)
                            .ThenInclude(pi => pi.Image)
                           .Include(pc => pc.ProductSizes)
                            .ThenInclude(ps => ps.Size)
                           .Include(pc => pc.Product)
                            .ThenInclude(p => p.Reviews)
                           .Include(pc => pc.ProductDiscounts)
                            .ThenInclude(pd => pd.Discount)
                           .AsSplitQuery()
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(pc => pc.Id == search).FirstOrDefault()
                            ?? throw new EntityNotFoundException(search, nameof(Product));

        var product = productColor.Product;

        var productDiscount = productColor.ProductDiscounts
            .Where(pd => pd.Discount.IsActive &&
                         (!pd.Discount.StartDate.HasValue || pd.Discount.StartDate <= DateTime.UtcNow) &&
                         (!pd.Discount.EndDate.HasValue || pd.Discount.EndDate >= DateTime.UtcNow))
            .Select(pd => pd.Discount)
            .FirstOrDefault();

        decimal? newPrice = null;
        string? discountType = null;
        decimal? discountValue = null;
        decimal oldPrice = product.Price;

        if (productDiscount != null)
        {
            newPrice = Math.Round(product.Price * (1 - productDiscount.Percentage / 100), 2);
            discountType = productDiscount.Name.ToLower().Contains("popusti") ? "Popust" : productDiscount.Name;
            discountValue = productDiscount.Percentage;
        }

        return new ProductDto
        {
            Id = productColor.Id,
            Name = product.Name,
            Brand = product.Brand.Name,
            Category = product.Category.Name,
            Color = productColor.Color.Name,
            Code = productColor.Code,
            AvgRating = product.Reviews.Any() ? Math.Round((decimal)product.Reviews.Average(r => r.Rating), 1) : 0,
            ReviewCount = product.Reviews.Any() ? product.Reviews.Count() : 0,
            OldPrice = oldPrice,
            NewPrice = newPrice,
            DiscountType = discountType,
            DiscountValue = discountValue,
            Images = productColor.ProductImages.Select(pi => $"/images/products/{Path.GetFileName(pi.Image.Path)}"),
            Sizes = productColor.ProductSizes.Select(ps => new ProductSizeDto
            {
                Id = ps.Size.Id,
                Number = ps.Size.Number,
                Detail = ps.Size.Detail,
                Quantity = ps.Quantity
            }),
            Variants = [.. Context.ProductColors
                .Where(pc => pc.ProductId == product.Id)
                .Select(pc => new ProductVariantDto
                {
                    Id = pc.Id,
                    Image = $"/images/products/{Path.GetFileName(pc.ProductImages
                        .Where(x => x.Image.Path.Contains("thumb"))
                        .Select(x => x.Image.Path)
                        .FirstOrDefault())}"
                })],
            IsFavorite = Context.Favorites.Any(f => f.UserId == User.Id && f.ProductColorId == productColor.Id)
        };
    }
}
