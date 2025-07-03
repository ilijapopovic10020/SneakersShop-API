using System;
using SneakersShop.Application.UseCases;

namespace SneakersShop.Application.Handling;

public interface ICommandHandler
{
    void HandleCommand<TRequest>(ICommand<TRequest> command, TRequest data);
}
