using System;

namespace SneakersShop.Application.Exceptions;

public class UseCaseConflictException : Exception
{
    public UseCaseConflictException(string message) : base(message)
    {
        
    }
}
