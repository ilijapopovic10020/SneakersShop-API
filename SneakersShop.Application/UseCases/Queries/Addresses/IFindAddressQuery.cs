using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Addresses;

public interface IFindAddressQuery : IQuery<int, AddressDto>
{ }
