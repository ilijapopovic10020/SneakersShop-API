using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Cities;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Cities;

public class EfGetCitiesQuery(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IGetCitiesQuery
{
    public int Id => 22;

    public string Name => "Search cities";

    public string Description => "";

    public IEnumerable<CityDto> Execute(BaseSearch search)
    {
        var query = Context.Cities.AsQueryable();

        if (!string.IsNullOrEmpty(search.Keyword))
        {
            query = query.Where(x => x.Name.Contains(search.Keyword));
        }

        return query.Select(x => new CityDto
        {
            Id = x.Id,
            ZipCode = x.ZipCode,
            Name = x.Name,
        });
    }
}
