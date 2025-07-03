using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Colors;

public interface IFindColorQuery : IQuery<int, ColorsDto>
{

}
