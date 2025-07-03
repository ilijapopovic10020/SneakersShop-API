using System;
using SneakersShop.Application.UseCases.DTO;

namespace SneakersShop.Application.UseCases.Queries.Users;

public interface IFindUserQuery : IQuery<int, UserDto>
{

}
