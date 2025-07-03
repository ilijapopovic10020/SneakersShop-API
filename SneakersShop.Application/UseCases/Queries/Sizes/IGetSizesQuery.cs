using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Sizes;

public interface IGetSizesQuery : IQuery<BaseSearch, IEnumerable<SizesDto>>
{

}
