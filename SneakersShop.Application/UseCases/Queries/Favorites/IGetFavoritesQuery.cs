using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Favorites;

public interface IGetFavoritesQuery : IQuery<FavoriteSearch, PagedResponse<ProductsDto>>
{

}