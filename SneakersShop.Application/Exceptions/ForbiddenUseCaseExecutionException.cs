using System;

namespace SneakersShop.Application.Exceptions;

public class ForbiddenUseCaseExecutionException(string useCase, string user) 
    : Exception($"User {user} has tried to execute {useCase} without being authorized to do so.")
{
}
