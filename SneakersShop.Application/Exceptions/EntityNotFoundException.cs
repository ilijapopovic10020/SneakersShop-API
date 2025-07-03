using System;

namespace SneakersShop.Application.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(int id, string entityType)
        : base($"Entity of type {entityType} with an id {id} is not found.") { }
}
