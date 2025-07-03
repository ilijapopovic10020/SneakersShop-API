using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Sizes;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Sizes;

public class EfFindSizeQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IFindSizeQuery
{
    public int Id => 11;

    public string Name => "Find Size";

    public string Description => "";

    public SizesDto Execute(int search)
    {
        var size = Context.Sizes.FirstOrDefault(x => x.Id == search);

        return new SizesDto
        {
            Id = size.Id,
            Number = size.Number,
            Detail = size.Detail
        };
    }
}
