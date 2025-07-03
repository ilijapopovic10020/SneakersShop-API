using System;
using SneakersShop.Application.UseCases;

namespace SneakersShop.Application.Handling;

public interface IQueryHandler
{
    TResponse HandleQuery<TRequest, TResponse>(IQuery<TRequest, TResponse> query, TRequest data);
    
    TResponse HandleQuery<TResponse>(IQuery<TResponse> query);
}
