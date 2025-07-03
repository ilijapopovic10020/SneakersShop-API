using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Products;

public interface IFindProductQuery : IQuery<int, ProductDto>
{

}
