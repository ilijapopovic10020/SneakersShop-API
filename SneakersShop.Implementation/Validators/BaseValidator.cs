using System;
using FluentValidation;
using SneakersShop.DataAccess;

namespace SneakersShop.Implementation.Validators;

public abstract class BaseValidator<T>(SneakersShopDbContext context) : AbstractValidator<T>
{
    protected readonly SneakersShopDbContext Context = context;
}
