using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries;
using SneakersShop.Application.UseCases.Queries.Products;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Products;

public class EfGetProductsQuery(SneakersShopDbContext context, IApplicationUser user)
    : EfUseCase(context, user),
        IGetProductsQuery
{
    public int Id => 5;

    public string Name => "Search Products";

    public string Description => "";

    public PagedResponse<ProductsDto> Execute(ProductsSearch search)
    {
        var query = Context.ProductOverview.AsQueryable();

        // FILTERI
        if (!string.IsNullOrEmpty(search.Keyword))
            query = query.Where(x => x.ProductName.Contains(search.Keyword));

        if (search.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == search.CategoryId.Value);

        if (search.BrandId != null && search.BrandId.Count > 0)
            query = query.Where(x =>
                search.BrandId != null && search.BrandId.Any(b => b == x.BrandId)
            );

        if (search.Color != null && search.Color.Count > 0)
            query = query.Where(x =>
                x.ColorName != null && search.Color.Any(c => c == x.ColorName)
            );

        if (search.MinPrice.HasValue)
            query = query.Where(x => x.NewPrice.HasValue ? x.NewPrice >= search.MinPrice.Value : x.OldPrice >= search.MinPrice.Value);

        if (search.MaxPrice.HasValue)
            query = query.Where(x => x.NewPrice.HasValue ? x.NewPrice <= search.MaxPrice.Value : x.OldPrice <= search.MaxPrice.Value);

        // SORTIRANJE
        query = search.Filter switch
        {
            Filter.BestSeller => query.OrderByDescending(x => x.SoldQuantity),
            Filter.Newest => query.OrderByDescending(x => x.CreatedAt),
            Filter.PriceLowToHigh => query.OrderBy(x => x.OldPrice),
            Filter.PriceHighToLow => query.OrderByDescending(x => x.OldPrice),
            Filter.BestRated => query.OrderByDescending(x => x.AvgRating),
            _ => query.OrderBy(x => x.ProductName),
        };

        // PAGINACIJA
        int perPage = search.PerPage ?? 15;
        int page = search.Page ?? 1;
        int skip = (page - 1) * perPage;

        var totalCount = query.Count();

        var data = query
            .Skip(skip)
            .Take(perPage)
            .Select(x => new ProductsDto
            {
                ProductColorId = x.ProductColorId,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                OldPrice = x.OldPrice,
                BrandId = x.BrandId,
                Brand = x.BrandName,
                CategoryId = x.CategoryId,
                Category = x.CategoryName,
                Color = x.ColorName,
                Code = x.Code,
                Image = x.ThumbnailPath,
                AvgRating = x.AvgRating,
                ReviewCount = x.ReviewCount,
                NewPrice = x.NewPrice,
                DiscountType = x.DiscountType,
                DiscountValue = x.DiscountValue,
                SoldQuantity = x.SoldQuantity,
                CreatedAt = x.CreatedAt,
            })
            .ToList();

        return new PagedResponse<ProductsDto>
        {
            TotalCount = totalCount,
            CurrentPage = page,
            ItemsPerPage = perPage,
            Data = data,
        };
    }
}
