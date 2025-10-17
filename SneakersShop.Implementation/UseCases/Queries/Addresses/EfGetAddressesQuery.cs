using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Addresses;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Addresses;

public class EfGetAddressesQuery(SneakersShopDbContext context, IApplicationUser? user)
    : EfUseCase(context, user),
        IGetAddressesQuery
{
    public int Id => 34;

    public string Name => "Search Addresses";

    public string Description => "";

    public IEnumerable<AddressesDto> Execute(BaseSearch search)
    {
        if (User == null)
        {
            throw new UnauthorizedAccessException("User must be logged in.");
        }

        var addresses = Context
            .Addresses.Include(a => a.City)
            .Where(x => x.UserId == User.Id)
            .Select(x => new AddressesDto
            {
                Id = x.Id,
                Street = x.Street,
                City = x.City.Name,
                ZipCode = x.City.ZipCode,
                IsDefault = x.IsDefault,
            });

        return addresses;
    }
}
