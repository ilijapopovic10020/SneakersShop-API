using System;
using System.Reflection.Metadata;
using SneakersShop.Application.Logging;

namespace SneakersShop.Implementation.Logging;

public class ConsoleExceptionLogger : IExceptionLogger
{
    public void Log(Exception ex)
    {
        System.Console.WriteLine("Occured at: " + DateTime.UtcNow);
        System.Console.WriteLine(ex.Message);
        System.Console.WriteLine(ex.InnerException);
    }
}
