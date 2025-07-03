using System;
using System.Diagnostics;
using Newtonsoft.Json;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.Handling;
using SneakersShop.Application.Logging;
using SneakersShop.Application.UseCases;
using SneakersShop.Domain;

namespace SneakersShop.Implementation.Handling;

public class CommandHandler(IExceptionLogger exceptionLogger, IApplicationUser user, IUseCaseLogger useCaseLogger) : ICommandHandler
{
    private readonly IExceptionLogger _exceptionLogger = exceptionLogger;
    private readonly IUseCaseLogger _useCaseLogger = useCaseLogger;
    private readonly IApplicationUser _user = user;

    public void HandleCommand<TRequest>(ICommand<TRequest> command, TRequest data)
    {
        try
        {
            var isAuthorized = _user.UseCaseIds.Contains(command.Id);
            
            var log = new UseCaseLog
            {
                User = _user.Identity,
                ExecutionDateTime = DateTime.UtcNow,
                UseCaseName = command.Name,
                UserId = _user.Id,
                Data = JsonConvert.SerializeObject(data),
                IsAuthorized = isAuthorized
            };

            _useCaseLogger.Log(log);

            if(!isAuthorized)
            {
                throw new ForbiddenUseCaseExecutionException(command.Name, _user.Identity);
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            command.Execute(data);

            stopWatch.Stop();
        }
        catch (Exception ex)
        {
            _exceptionLogger.Log(ex);
            throw;
        }
    }
}
