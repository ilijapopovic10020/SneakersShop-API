using System;
using SneakersShop.Application.UseCases.Commands.Addresses;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Addresses;

public class EfCreateAddressCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), ICreateAddressCommand
{
    public int Id => 23;

    public string Name => "Create address";

    public string Description => "";

    public void Execute(CreateAddressDto request)
    {
        var userAddresses = Context.Addresses
        .Where(x => x.UserId == User.Id)
        .ToList();

    if (userAddresses.Count >= 3)
    {
        throw new Exception("Maksimalan broj adresa je 3.");
    }

    var isDefault = userAddresses.Count == 0;

    var address = new Address
    {
        UserId = User.Id,
        Street = request.Street,
        CityId = request.CityId,
        IsDefault = isDefault
    };

    Context.Addresses.Add(address);
    Context.SaveChanges();
    }
}
