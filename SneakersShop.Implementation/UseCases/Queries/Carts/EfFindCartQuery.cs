using SneakersShop.Application.UseCases.Queries.Carts;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Carts;

public class EfFindCartQuery(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IFindCartQuery
{
    public int Id => 35;

    public string Name => "Find Cart";

    public string Description => "";

    public string Execute()
    {
        if (User == null || User.Id == 0)
            throw new UnauthorizedAccessException();

        var cart = Context.Carts.FirstOrDefault(x => x.UserId == User.Id);

        return cart?.CartItems ?? "[]";
    }
}
