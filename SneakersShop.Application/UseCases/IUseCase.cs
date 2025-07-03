using System;

namespace SneakersShop.Application.UseCases;

public interface IUseCase
{
    public int Id { get; }
    string Name { get; }
    string Description { get; }
}
