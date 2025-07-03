using System;
using System.Runtime.CompilerServices;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Categories;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Categories;

public class EfFindCategoryQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IFindCategoryQuery
{
    public int Id => 9;

    public string Name => "Find Category";

    public string Description => "";

    public CategoriesDto Execute(int search)
    {
        var category = Context.Categories.FirstOrDefault(x => x.Id == search && x.IsActive);

        return new CategoriesDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
