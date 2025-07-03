using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Users;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;
using SneakersShop.Implementation.UseCases;

namespace SneakersShop.Implementation.Validators.Users;

public class EfDeleteUserCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IDeleteUserCommand
{
    public int Id => 25;

    public string Name => "Delete user";

    public string Description => "";

    public void Execute(int request)
    {
        var userToDelete = Context.Users.FirstOrDefault(x => x.Id == request) ?? throw new EntityNotFoundException(request, nameof(Domain.Entities.User));

        if (!User.UseCaseIds.Contains(Id))
            throw new ForbiddenUseCaseExecutionException(User.Identity, nameof(Name));

        var addresses = Context.Addresses.Where(x => x.UserId == userToDelete.Id).ToList();
        var orders = Context.Orders.Where(x => x.UserId == userToDelete.Id).ToList();
        var reviews = Context.Reviews.Where(x => x.UserId == userToDelete.Id).ToList();
        var favorites = Context.Favorites.Where(x => x.UserId == userToDelete.Id).ToList();


        Context.RemoveRange(addresses);
        Context.RemoveRange(orders);
        Context.RemoveRange(reviews);
        context.RemoveRange(favorites);
        Context.Users.Remove(userToDelete);
        Context.SaveChanges();
    }
}
