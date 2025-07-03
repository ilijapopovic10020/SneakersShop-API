using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Products;

public interface IGetProductsQuery : IQuery<ProductsSearch, PagedResponse<ProductsDto>> { }



