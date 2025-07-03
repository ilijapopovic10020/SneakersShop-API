using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Brands;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Brands;

public class EfGetBrandsQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IGetBrandsQuery
{
    public int Id => 1;

    public string Name => "Search Brands";

    public string Description => "Search Brands using Entity Framework.";

    public IEnumerable<BrandDto> Execute(BaseSearch search)
    {
        var query = Context.Brands.AsQueryable();

        if (!string.IsNullOrEmpty(search.Keyword))
        {
            query = query.Where(x => x.Name.Contains(search.Keyword));
        }

        return query.Select(x => new BrandDto
        {
            Id = x.Id,
            Name = x.Name,
            Image = $"/images/brands/{Path.GetFileName(x.Image.Path)}"
        });
    }
}
