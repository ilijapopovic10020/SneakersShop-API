using System;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases;

public abstract class EfUseCase(SneakersShopDbContext context, IApplicationUser? user)
{
    protected SneakersShopDbContext Context { get; } = context;
    protected IApplicationUser User { get; } = user;
}
