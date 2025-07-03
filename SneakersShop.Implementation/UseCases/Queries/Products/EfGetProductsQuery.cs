using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries;
using SneakersShop.Application.UseCases.Queries.Products;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Products;

public class EfGetProductsQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IGetProductsQuery
{
    public int Id => 5;

    public string Name => "Search Products";

    public string Description => "";

    public PagedResponse<ProductsDto> Execute(ProductsSearch search)
    {
        var query = Context.ProductColors
            .Include(pc => pc.Product)
                .ThenInclude(p => p.Brand)
            .Include(pc => pc.Product)
                .ThenInclude(p => p.Category)
            .Include(pc => pc.Product)
                .ThenInclude(p => p.Reviews)
            .Include(pc => pc.Color)
            .Include(pc => pc.ProductImages)
                .ThenInclude(pi => pi.Image)
            .Include(pc => pc.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
            .AsSplitQuery()
            .AsNoTracking()
            .AsQueryable();

        // Filteri
        if (!string.IsNullOrEmpty(search.Keyword))
        {
            query = query.Where(x => x.Product.Name.Contains(search.Keyword));
        }

        if (search.CategoryId.HasValue)
        {
            query = query.Where(x => x.Product.CategoryId == search.CategoryId.Value);
        }

        if (search.BrandId != null && search.BrandId.Any())
        {
            query = query.Where(x => search.BrandId.Contains(x.Product.BrandId));
        }

        if (search.Color != null && search.Color.Any())
        {
            query = query.Where(x => search.Color.Contains(x.Color.Name));
        }

        if (search.MinPrice.HasValue)
        {
            query = query.Where(x => x.Product.Price >= search.MinPrice.Value);
        }

        if (search.MaxPrice.HasValue)
        {
            query = query.Where(x => x.Product.Price <= search.MaxPrice.Value);
        }

        // Sortiranje
        switch (search.Filter)
        {
            case Filter.BestSeller:
                query = query.OrderByDescending(x => x.Product.Reviews.Count());
                break;
            case Filter.Newest:
                query = query.OrderByDescending(x => x.Product.CreatedAt);
                break;
            case Filter.PriceLowToHigh:
                query = query.OrderBy(x => x.Product.Price);
                break;
            case Filter.PriceHighToLow:
                query = query.OrderByDescending(x => x.Product.Price);
                break;
            case Filter.BestRated:
                query = query.OrderByDescending(x =>
                    x.Product.Reviews.Any() ? x.Product.Reviews.Average(r => r.Rating) : 0);
                break;
            default:
                query = query.OrderBy(x => x.Product.Name);
                break;
        }

        // Paginacija
        int perPage = search.PerPage ?? 15;
        int page = search.Page ?? 1;
        int skip = (page - 1) * perPage;

        var totalCount = query.Count();

        var data = query
            .Skip(skip)
            .Take(perPage)
            .ToList()
            .Select(x =>
            {
                var productDiscount = x.ProductDiscounts
                    .Where(pd => pd.Discount.IsActive)
                    .Select(pd => pd.Discount)
                    .FirstOrDefault();

                decimal? newPrice = null;
                string? discountType = null;
                decimal? discountValue = null;
                decimal oldPrice = x.Product.Price;

                if (productDiscount != null)
                {
                    newPrice = Math.Round(x.Product.Price * (1 - productDiscount.Percentage / 100), 2);
                    discountType = productDiscount.Name.ToLower().Contains("popusti") ? "Popust" : productDiscount.Name;
                    discountValue = productDiscount.Percentage;
                }

                var imagePath = x.ProductImages
                    .Where(img => img.Image.Path.Contains("thumb"))
                    .Select(img => img.Image.Path)
                    .FirstOrDefault();

                return new ProductsDto
                {
                    Id = x.Id,
                    Name = x.Product.Name,
                    Brand = x.Product.Brand.Name,
                    Category = x.Product.Category.Name,
                    AvgRating = x.Product.Reviews.Any() ? Math.Round(x.Product.Reviews.Average(r => r.Rating), 1) : 0,
                    ReviewCount = x.Product.Reviews.Count(),
                    OldPrice = oldPrice,
                    NewPrice = newPrice,
                    DiscountType = discountType,
                    DiscountValue = discountValue,
                    Color = x.Color.Name,
                    Code = x.Code,
                    Image = imagePath != null ? $"/images/products/{Path.GetFileName(imagePath)}" : string.Empty
                };
            }).ToList();

        return new PagedResponse<ProductsDto>
        {
            TotalCount = totalCount,
            CurrentPage = page,
            ItemsPerPage = perPage,
            Data = data
        };
    }
}
