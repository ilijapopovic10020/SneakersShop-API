using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Colors;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Colors;

public class EfGetColorsQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IGetColorsQuery
{
    public int Id => 12;

    public string Name => "Search Colors";

    public string Description => "";

    public IEnumerable<ColorsDto> Execute(BaseSearch search)
    {
        var colors = Context.Colors.AsQueryable();

        return colors.Select(x => new ColorsDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }
}
