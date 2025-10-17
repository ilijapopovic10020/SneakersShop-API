using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries;
using SneakersShop.Application.UseCases.Queries.Favorites;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Favorites;

public class EfGetFavoritesQuery(SneakersShopDbContext context, IApplicationUser? user)
    : EfUseCase(context, user),
        IGetFavoritesQuery
{
    public int Id => 32;

    public string Name => "Search favorites";

    public string Description => "";

    public PagedResponse<ProductsDto> Execute(FavoriteSearch search)
    {
        if (User == null)
            throw new UnauthorizedAccessException();

        var favoriteProductColorIds = Context
            .Favorites.Where(f => f.UserId == User.Id)
            .Select(f => f.ProductColorId)
            .ToList();

        var products = Context
            .ProductColors.Include(x => x.Product)
            .ThenInclude(p => p.Brand)
            .Include(x => x.Product)
            .ThenInclude(p => p.Category)
            .Include(x => x.Product)
            .ThenInclude(p => p.Reviews)
            .Include(x => x.Color)
            .Include(x => x.ProductImages)
            .ThenInclude(pi => pi.Image)
            .Include(x => x.ProductDiscounts)
            .ThenInclude(pd => pd.Discount)
            .Where(x => favoriteProductColorIds.Contains(x.Id))
            .AsQueryable();

        if (!string.IsNullOrEmpty(search.Keyword))
        {
            products = products.Where(x => x.Product.Name.Contains(search.Keyword));
        }

        if (search.PerPage == null || search.PerPage < 1)
        {
            search.PerPage = 15;
        }

        if (search.Page == null || search.Page < 1)
        {
            search.Page = 1;
        }

        var skip = (search.Page.Value - 1) * search.PerPage.Value;

        var response = new PagedResponse<ProductsDto>
        {
            TotalCount = products.Count(),
            CurrentPage = search.Page.Value,
            ItemsPerPage = search.PerPage.Value,
            Data =
            [
                .. products
                    .Skip(skip)
                    .Take(search.PerPage.Value)
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var productDiscount = x
                            .ProductDiscounts.Where(pd =>
                                pd.Discount.IsActive
                                && (
                                    !pd.Discount.StartDate.HasValue
                                    || pd.Discount.StartDate <= DateTime.UtcNow
                                )
                                && (
                                    !pd.Discount.EndDate.HasValue
                                    || pd.Discount.EndDate >= DateTime.UtcNow
                                )
                            )
                            .Select(pd => pd.Discount)
                            .FirstOrDefault();

                        decimal? newPrice = null;
                        string? discountType = null;
                        decimal? discountValue = null;
                        decimal oldPrice = x.Product.Price;

                        if (productDiscount != null)
                        {
                            newPrice = Math.Round(
                                x.Product.Price * (1 - productDiscount.Percentage / 100),
                                2
                            );
                            discountType = productDiscount.Name.ToLower().Contains("popusti")
                                ? "Popust"
                                : productDiscount.Name;
                            discountValue = productDiscount.Percentage;
                        }

                        var imagePath = x
                            .ProductImages.Where(img => img.Image.Path.Contains("thumb"))
                            .Select(img => img.Image.Path)
                            .FirstOrDefault();

                        return new ProductsDto
                        {
                            ProductColorId = x.Id,
                            ProductName = x.Product.Name,
                            Brand = x.Product.Brand.Name,
                            Category = x.Product.Category.Name,
                            AvgRating =
                                x.Product.Reviews.Count != 0
                                    ? Math.Round(x.Product.Reviews.Average(r => r.Rating), 1)
                                    : 0,
                            ReviewCount =
                                x.Product.Reviews.Count != 0 ? x.Product.Reviews.Count : 0,
                            OldPrice = oldPrice,
                            NewPrice = newPrice,
                            DiscountType = discountType,
                            DiscountValue = discountValue,
                            Color = x.Color.Name,
                            Code = x.Code,
                            Image =
                                imagePath != null
                                    ? $"/images/products/{Path.GetFileName(imagePath)}"
                                    : string.Empty,
                        };
                    }),
            ],
        };

        return response;
    }
}
