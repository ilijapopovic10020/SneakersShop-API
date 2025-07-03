using System;
using SneakersShop.Application.UseCases.Commands.Addresses;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Commands.Addresses;

public class EfDeleteAddressCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IDeleteAddressCommand
{
    public int Id => 26;

    public string Name => "Delete address";

    public string Description => "";

    public void Execute(int request)
    {
        var address = Context.Addresses.FirstOrDefault(x => x.Id == request) ?? throw new InvalidOperationException("Address not found.");

        if (User == null || address.UserId != User.Id)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this address.");
        }

        Context.Addresses.Remove(address);
        Context.SaveChanges();
    }
}
