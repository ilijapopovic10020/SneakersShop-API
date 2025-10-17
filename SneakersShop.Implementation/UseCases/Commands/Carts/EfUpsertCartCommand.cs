using System;
using System.Text.Json;
using SneakersShop.Application.UseCases.Commands.Carts;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Carts;

public class EfUpsertCartCommand(SneakersShopDbContext context, IApplicationUser? user)
    : EfUseCase(context, user),
        IUpsertCartCommand
{
    public int Id => 36;

    public string Name => "Upsert Cart";

    public string Description => "";

    public void Execute(List<CartDto> request)
    {
        if (User == null || User.Id == 0)
            throw new UnauthorizedAccessException("User must be authenticated to save their cart.");

        var cartItemsJson = JsonSerializer.Serialize(request);

        var cart = Context.Carts.FirstOrDefault(x => x.UserId == User.Id);

        if (cart != null)
        {
            cart.CartItems = cartItemsJson;
            cart.ModifiedAt = DateTime.UtcNow;
            cart.ModifiedBy = User.Identity;
        }
        else
        {
            cart = new Cart
            {
                UserId = User.Id,
                CartItems = cartItemsJson,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };

            Context.Carts.Add(cart);
        }

        Context.SaveChanges();
    }
}
