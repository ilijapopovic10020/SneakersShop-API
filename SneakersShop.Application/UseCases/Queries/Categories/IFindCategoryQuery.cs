using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Categories;

public interface IFindCategoryQuery : IQuery<int, CategoriesDto>
{

}
