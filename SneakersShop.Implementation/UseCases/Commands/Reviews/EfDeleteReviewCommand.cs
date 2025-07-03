using System;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.UseCases.Commands.Reviews;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Implementation.UseCases.Commands.Reviews;

public class EfDeleteReviewCommand(SneakersShopDbContext context, IApplicationUser user) : EfUseCase(context, user), IDeleteReviewCommand
{
    public int Id => 17;

    public string Name => "Delete Review";

    public string Description => "";

    public void Execute(int request)
    {
        var review = Context.Reviews.FirstOrDefault(x => x.Id == request && x.IsActive);

        if(review == null)
        {
            throw new EntityNotFoundException(request, nameof(Review));
        }

        if(review.UserId != User.Id)
        {
            throw new ForbiddenUseCaseExecutionException(Name, User.Identity);
        }

        review.IsActive = false;
        review.DeletedAt = DateTime.UtcNow;
        review.DeletedBy = User.Identity;

        Context.SaveChanges();
    }
}
