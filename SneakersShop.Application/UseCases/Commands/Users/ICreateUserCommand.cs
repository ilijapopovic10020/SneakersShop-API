using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Commands.Users;

public interface ICreateUserCommand : ICommand<CreateUserDto>
{

}
