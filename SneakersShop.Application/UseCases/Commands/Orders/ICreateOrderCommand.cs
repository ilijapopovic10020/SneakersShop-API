using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Commands.Orders;

public interface ICreateOrderCommand : ICommand<CreateOrderDto>
{

}
