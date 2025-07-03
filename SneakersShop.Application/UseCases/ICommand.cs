using System;

namespace SneakersShop.Application.UseCases;

public interface ICommand<T> : IUseCase
{
    void Execute(T request);
}
