using Microsoft.EntityFrameworkCore;
using SneakersShop.DataAccess.Exceptions;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Extensions;

public static class DbSetExtensions
{
    public static void Deactivate(this DbContext context, Entity entity)
    {
        entity.IsActive = false;
        context.Entry(entity).State = EntityState.Modified;
    }

    public static void Deactivate<T>(this SneakersShopDbContext context, int id)
        where T : Entity
    {
        var itemToDeactivate = context.Set<T>().Find(id);

        if (itemToDeactivate == null)
        {
            throw new EntityNotFoundExcpetion();
        }

        itemToDeactivate.IsActive = false;
    }
}
