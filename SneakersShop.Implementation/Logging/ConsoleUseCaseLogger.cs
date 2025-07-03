using System;
using SneakersShop.Application.Logging;

namespace SneakersShop.Implementation.Logging;

public class ConsoleUseCaseLogger : IUseCaseLogger
{
    public void Log(UseCaseLog log)
    {
        Console.WriteLine($"UseCase: {log.UseCaseName}" +
                          $"\n User: {log.User}" +
                          $"\n Time: {log.ExecutionDateTime}" +
                          $"\n Authorized: {log.IsAuthorized}" +
                          $"\n Data: \n{log.Data}\n\n");
    }
}
