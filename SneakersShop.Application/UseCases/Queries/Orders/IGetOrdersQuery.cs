using System;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.DTO.Searches;

namespace SneakersShop.Application.UseCases.Queries.Orders;

public interface IGetOrdersQuery : IQuery<PagedSearchId, IEnumerable<OrdersDto>>
{

}
