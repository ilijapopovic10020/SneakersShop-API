using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Brands;

public interface IGetBrandsQuery : IQuery<BaseSearch, IEnumerable<BrandDto>>
{
}
