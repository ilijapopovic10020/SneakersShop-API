using System;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Orders;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Orders;

public class EfDeleteOrderCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IDeleteOrderCommand
{
    public int Id => 29;

    public string Name => "Delete Order";

    public string Description => "";

    public void Execute(int request)
    {
        var order = Context.Orders.FirstOrDefault(x => x.Id == request) ?? throw new EntityNotFoundException(request, nameof(Order));
        
        if (order.UserId != User?.Id)
        {
            throw new ForbiddenUseCaseExecutionException(nameof(Name), User.Identity);
        }

        Context.Orders.Remove(order);
        Context.SaveChanges();
    }
}
