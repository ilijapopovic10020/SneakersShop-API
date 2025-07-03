using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Commands.Brands;

public interface ICreateBrandCommand : ICommand<CreateBrandDto>
{
}
