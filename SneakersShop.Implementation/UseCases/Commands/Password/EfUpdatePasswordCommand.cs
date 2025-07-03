using System;
using SneakersShop.Application.UseCases.Commands.Password;
using SneakersShop.Application.UseCases.DTO;
using SneakersShop.DataAccess;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.UseCases.Commands.Password;

public class EfUpdatePasswordCommand(SneakersShopDbContext context, IApplicationUser? user) : EfUseCase(context, user), IUpdatePasswordCommand
{
    public int Id => 37;

    public string Name => "Update password";

    public string Description => "";

    public void Execute(UpdatePasswordDto request)
    {
        var userEntity = Context.Users.FirstOrDefault(x => User.Id == request.Id || x.Id == request.Id);

        if (userEntity == null)
        {
            throw new UnauthorizedAccessException("Korisnik nije pronađen.");
        }

        // Proveri da li je stara lozinka ispravna
        var isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.OldPassword, userEntity.Password);
        if (!isOldPasswordCorrect)
        {
            throw new Exception("Stara lozinka nije ispravna.");
        }

        // Proveri da nova lozinka nije ista kao stara
        var isSameAsOld = BCrypt.Net.BCrypt.Verify(request.NewPassword, userEntity.Password);
        if (isSameAsOld)
        {
            throw new Exception("Nova lozinka ne sme biti ista kao prethodna.");
        }

        // Hesuj novu lozinku i snimi promenu
        userEntity.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        Context.SaveChanges();
    }
}
