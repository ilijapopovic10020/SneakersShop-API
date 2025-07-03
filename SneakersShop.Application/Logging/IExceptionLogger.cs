using System;

namespace SneakersShop.Application.Logging;

public interface IExceptionLogger
{
    void Log(Exception ex);
}
