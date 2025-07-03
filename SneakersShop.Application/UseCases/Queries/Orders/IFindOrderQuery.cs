using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Orders;

public interface IFindOrderQuery : IQuery<int, OrderDto>
{

}
