using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.Application.UseCases.Queries.Categories;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Categories;

public class EfGetCategoriesQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IGetCategoriesQuery
{
    public int Id => 8;

    public string Name => "Search Categories";

    public string Description => "";

    public IEnumerable<CategoriesDto> Execute(BaseSearch search)
    {
        var categories = Context.Categories.AsQueryable();

        if (!string.IsNullOrEmpty(search.Keyword))
        {
            categories = categories.Where(x => x.Name.Contains(search.Keyword));
        }

        return categories.Select(x => new CategoriesDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }
}
