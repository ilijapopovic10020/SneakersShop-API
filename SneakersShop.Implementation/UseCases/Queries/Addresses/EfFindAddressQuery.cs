using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Addresses;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Addresses;

public class EfFindAddressQuery(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IFindAddressQuery
{
    public int Id => 38;

    public string Name => "Find Address";

    public string Description => "";

    public AddressDto Execute(int search)
    {
        var address = Context.Addresses
                             .Include(x => x.City)
                             .Where(x => x.UserId == User.Id && x.Id == search)
                             .FirstOrDefault() ?? throw new EntityNotFoundException(search, nameof(Address));

        return new AddressDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City.Name,
            ZipCode = address.City.ZipCode,
            IsDefault = address.IsDefault
        };
    }
}
