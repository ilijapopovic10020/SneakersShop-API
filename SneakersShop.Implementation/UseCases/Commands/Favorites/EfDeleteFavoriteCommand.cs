using System;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Favorites;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Favorites;

public class EfDeleteFavoriteCommand : EfUseCase, IDeleteFavoriteCommand
{
    public EfDeleteFavoriteCommand(SneakersShopDbContext context, IApplicationUser? user) : base(context, user)
    {
    }

    public int Id => 33;

    public string Name => "Delete favorite";

    public string Description => "";

    public void Execute(int request)
    {
        var favorite = Context.Favorites.FirstOrDefault(x => x.ProductColorId == request && x.UserId == User.Id)
                ?? throw new EntityNotFoundException(request, nameof(Favorite));
        
        Context.Favorites.Remove(favorite);
        Context.SaveChanges();
    }
}
