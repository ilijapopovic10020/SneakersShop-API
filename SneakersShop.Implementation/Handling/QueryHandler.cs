using System;
using System.Diagnostics;
using Newtonsoft.Json;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.Handling;
using SneakersShop.Application.Logging;
using SneakersShop.Application.UseCases;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.Handling;

public class QueryHandler(IExceptionLogger exceptionLogger, IApplicationUser user, IUseCaseLogger useCaseLogger) : IQueryHandler
{
    private readonly IExceptionLogger _exceptionLogger = exceptionLogger;
    private readonly IApplicationUser _user = user;
    private readonly IUseCaseLogger _useCaseLogger = useCaseLogger;

    public TResponse HandleQuery<TRequest, TResponse>(IQuery<TRequest, TResponse> query, TRequest data)
    {
        try
        {
            var isAuthorized = _user.UseCaseIds.Contains(query.Id);
            
            var log = new UseCaseLog
            {
                User = _user.Identity,
                ExecutionDateTime = DateTime.UtcNow,
                UseCaseName = query.Name,
                UserId = _user.Id,
                Data = JsonConvert.SerializeObject(data),
                IsAuthorized = isAuthorized
            };

            _useCaseLogger.Log(log);

            if(!isAuthorized)
            {
                throw new ForbiddenUseCaseExecutionException(query.Name, _user.Identity);
            }
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var response = query.Execute(data);

            stopWatch.Stop();

            return response;
        }
        catch (Exception ex)
        {
            _exceptionLogger.Log(ex);
            throw;
        }
    }

    public TResponse HandleQuery<TResponse>(IQuery<TResponse> query)
    {
        try
        {
            var isAuthorized = _user.UseCaseIds.Contains(query.Id);

            var log = new UseCaseLog
            {
                User = _user.Identity,
                ExecutionDateTime = DateTime.UtcNow,
                UseCaseName = query.Name,
                UserId = _user.Id,
                Data = "No request data",
                IsAuthorized = isAuthorized
            };

            _useCaseLogger.Log(log);

            if (!isAuthorized)
            {
                throw new ForbiddenUseCaseExecutionException(query.Name, _user.Identity);
            }

            var stopwatch = Stopwatch.StartNew();

            var response = query.Execute();

            stopwatch.Stop();

            return response;
        }
        catch (Exception ex)
        {
            _exceptionLogger.Log(ex);
            throw;
        }
    }
}
