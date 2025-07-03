using System;
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Brands;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Queries.Brands;

public class EfFindBrandQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IFindBrandQuery
{
    public int Id => 3;

    public string Name => "Find Brand";

    public string Description => "";

    public BrandDto Execute(int search)
    {
        var brand = Context.Brands.Include(b => b.Image).FirstOrDefault(x => x.Id == search && x.IsActive);

        if (brand == null)
        {
            throw new EntityNotFoundException(search, nameof(Brand));
        }

        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Image = brand.Image.Path
        };
    }
}
