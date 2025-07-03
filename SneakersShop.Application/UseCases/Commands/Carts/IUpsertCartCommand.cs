using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Commands.Carts;

public interface IUpsertCartCommand : ICommand<List<CartDto>>
{

}
