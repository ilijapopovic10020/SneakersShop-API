using System;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Addresses;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Addresses;

public class EfUpdateAddressCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IUpdateAddressCommand
{
    public int Id => 27;

    public string Name => "Update address";

    public string Description => "";

    public void Execute(UpdateAddressDto request)
    {
        var currentAddress = Context.Addresses.FirstOrDefault(x => x.Id == request.Id) ?? throw new EntityNotFoundException(request.Id, nameof(Address));

        if (User == null || currentAddress.UserId != User.Id)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this address.");
        }

        if (!string.IsNullOrWhiteSpace(request.Street))
            if (currentAddress.Street != request.Street)
                currentAddress.Street = request.Street;

        if (request.CityId != null)
        {
            var city = Context.Cities.FirstOrDefault(x => x.Id == request.CityId) ?? throw new EntityNotFoundException(request.CityId, nameof(City));

            if (currentAddress.CityId != request.CityId)
                currentAddress.CityId = request.CityId;
        }

        if (request.IsDefault)
        {
            var existingDefault = Context.Addresses
                .FirstOrDefault(x => x.UserId == User.Id && x.IsDefault && x.Id != currentAddress.Id);

            if (existingDefault != null)
            {
                existingDefault.IsDefault = false;
            }

            currentAddress.IsDefault = true;
        }

        Context.Addresses.Update(currentAddress);
        Context.SaveChanges();
    }
}
