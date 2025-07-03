using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Sizes;

public interface IFindSizeQuery : IQuery<int, SizesDto>
{

}
