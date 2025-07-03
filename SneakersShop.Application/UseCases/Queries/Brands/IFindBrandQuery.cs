using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Brands;

public interface IFindBrandQuery : IQuery<int, BrandDto>
{
}
