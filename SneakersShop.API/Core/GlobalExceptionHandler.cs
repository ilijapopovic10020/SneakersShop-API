using System.Net;
using FluentValidation;
using SneakersShop.API.Exceptions;
using SneakersShop.Application.Exceptions;
using SneakersShop.Application.Logging;

namespace SneakersShop.API.Core;

public class GlobalExceptionHandler(RequestDelegate next, IExceptionLogger logger)
{
    private readonly RequestDelegate _next = next;
    private readonly IExceptionLogger _logger = logger;

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (System.Exception ex)
        {
            _logger.Log(ex);
            httpContext.Response.ContentType = "application/json";
            var statusCode = GetStatusCode(ex);
            object response = CreateErrorResponse(ex, statusCode);

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }

    private static HttpStatusCode GetStatusCode(Exception ex) => ex switch
    {
        BadHttpRequestException => HttpStatusCode.BadRequest,
        ForbiddenUseCaseExecutionException => HttpStatusCode.Forbidden,
        UnauthorizedAccessException => HttpStatusCode.Unauthorized,
        EntityNotFoundException => HttpStatusCode.NotFound,
        ValidationException => HttpStatusCode.UnprocessableEntity,
        UserNotFoundException => HttpStatusCode.UnprocessableEntity,
        UseCaseConflictException => HttpStatusCode.Conflict,
        NotSupportedException => HttpStatusCode.MethodNotAllowed,
        _ => HttpStatusCode.InternalServerError
    };

    private static object CreateErrorResponse(Exception ex, HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => new { message = "Nešto nije u redu sa vašim unosom." },
            HttpStatusCode.Forbidden => new { message = "Stranica koju tražite ne postoji." },
            HttpStatusCode.Unauthorized => new { message = "Neophodna je prijava da biste pristupili ovom sadržaju." },
            HttpStatusCode.NotFound => new { message = "Stranica koju tražite ne postoji." },
            HttpStatusCode.UnprocessableEntity when ex is UserNotFoundException ue => new { message = "Korisničko ime ili lozinka nisu ispravni." },
            HttpStatusCode.UnprocessableEntity when ex is ValidationException ve => new
            {
                errors = ve.Errors.Select(x => new
                {
                    property = x.PropertyName,
                    error = x.ErrorMessage
                })
            },
            HttpStatusCode.Conflict => new { message = "Zahtev nije mogao biti dovršen zbog konflikta sa postojećim stanjem." },
            HttpStatusCode.MethodNotAllowed => new { message = "Ova radnja nije podržana" },
            HttpStatusCode.TooManyRequests => new { message = "Previše zahteva. Molimo pokušajte kasnije." },
            HttpStatusCode.BadGateway => new { message = "Server je trenutno nedostupan. Pokušajte kasnije." },
            HttpStatusCode.ServiceUnavailable => new { message = "Sistem je privremeno nedostupan. Pokušajte ponovo kasnije." },
            HttpStatusCode.GatewayTimeout => new { message = "Vreme za obradu zahteva je isteklo. Molimo pokušajte kasnije." },
            HttpStatusCode.InternalServerError => new { message = "Došlo je do greške na serveru. Pokušajte ponovo kasnije." },
            _ => new { message = "Neočekivana greška. Pokušajte ponovo kasnije." }
        };
    }
}
