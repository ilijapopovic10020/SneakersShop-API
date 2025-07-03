using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Sizes;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Sizes;

public class EfGetSizesQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IGetSizesQuery
{
    public int Id => 10;

    public string Name => "Search Sizes";

    public string Description => "";

    public IEnumerable<SizesDto> Execute(BaseSearch search)
    {
        var sizes = Context.Sizes.AsQueryable();

        return sizes.Select(x => new SizesDto
        {
            Id = x.Id,
            Number = x.Number,
            Detail = x.Detail
        });
    }
}
