
using Microsoft.EntityFrameworkCore;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.Application.UseCases.Queries.Users;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Queries.Users;

public class EfFindUserQuery(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IFindUserQuery
{
    public int Id => 14;

    public string Name => "Find User";

    public string Description => "";

    public UserDto Execute(int search)
    {
        var user = Context.Users.Include(u => u.Addresses)
                                .ThenInclude(a => a.City)
                                .Include(u => u.Image)
                                .FirstOrDefault(x => x.Id == search && x.IsActive)
                                ?? throw new EntityNotFoundException(search, nameof(User));

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            Phone = user.Phone,
            Image = $"/images/profile/{Path.GetFileName(user.Image.Path)}",
            Addresses = user.Addresses.Select(x => new AddressDto
            {
                Id = x.Id,
                City = x.City.Name,
                ZipCode = x.City.ZipCode,
                Street = x.Street,
                IsDefault = x.IsDefault,
            })
        };
    }
}
