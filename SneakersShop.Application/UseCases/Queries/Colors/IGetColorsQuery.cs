using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Colors;

public interface IGetColorsQuery : IQuery<BaseSearch, IEnumerable<ColorsDto>>
{

}
