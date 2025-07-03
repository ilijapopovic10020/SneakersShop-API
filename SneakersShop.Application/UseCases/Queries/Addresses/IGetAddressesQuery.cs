using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Addresses;

public interface IGetAddressesQuery : IQuery<BaseSearch, IEnumerable<AddressDto>>
{
    
}
