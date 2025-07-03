using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Colors;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Colors;

public class EfFindColorQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IFindColorQuery
{
    public int Id => 13;

    public string Name => "Find Color";

    public string Description => "";

    public ColorsDto Execute(int search)
    {
        var color = Context.Colors.FirstOrDefault(x => x.Id == search && x.IsActive);

        return new ColorsDto
        {
            Id = color.Id,
            Name = color.Name
        };
    }
}
