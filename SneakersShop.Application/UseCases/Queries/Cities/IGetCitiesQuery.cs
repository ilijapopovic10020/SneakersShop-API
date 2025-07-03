using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Cities;

public interface IGetCitiesQuery : IQuery<BaseSearch, IEnumerable<CityDto>>
{
}
