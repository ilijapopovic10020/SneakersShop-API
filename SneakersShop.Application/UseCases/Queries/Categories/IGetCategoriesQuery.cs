using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Categories;

public interface IGetCategoriesQuery : IQuery<BaseSearch, IEnumerable<CategoriesDto>>
{

}
