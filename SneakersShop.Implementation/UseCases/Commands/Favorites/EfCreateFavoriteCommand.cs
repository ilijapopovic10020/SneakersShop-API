using System;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Favorites;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Favorites;

public class EfCreateFavoriteCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), ICreateFavoriteCommand
{
    public int Id => 31;

    public string Name => "Create Favorite";

    public string Description => "";

    public void Execute(CreateFavoriteDto request)
    {
        var productColor = Context.ProductColors.FirstOrDefault(x=>x.Id == request.ProductColorId) ?? throw new EntityNotFoundException(request.ProductColorId, nameof(ProductColor));

        if (User == null)
            throw new UnauthorizedAccessException();

            if (Context.Favorites.Any(f => f.UserId == User.Id && f.ProductColorId == productColor.Id))
                throw new MessageException("Ovaj proizvod je već u omiljenima.");

        var favorite = new Favorite
        {
            UserId = User.Id,
            ProductColorId = productColor.Id
        };

        Context.Favorites.Add(favorite);
        Context.SaveChanges();
    }
}
