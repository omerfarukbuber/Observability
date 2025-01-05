using Observability.Shared;

namespace Observability.Api.Infrastructure;

public static class CustomResults
{
    private static IHttpContextAccessor? _contextAccessor;
    public static void Configure(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            title: GetTitle(result.Error),
            detail: GetDetail(result.Error),
            type: GetType(result.Error),
            statusCode: GetStatusCode(result.Error),
            instance: GetInstance(),
            extensions: GetErrors(result)
            );

        static string GetTitle(Error error) => error.Type switch
        {
            ErrorType.Failure => error.Code,
            ErrorType.Validation => error.Code,
            ErrorType.Problem => error.Code,
            ErrorType.NotFound => error.Code,
            ErrorType.Conflict => error.Code,
            _ => "Server failure"
        };

        static string GetDetail(Error error) => error.Type switch
        {
            ErrorType.Failure => error.Description,
            ErrorType.Validation => error.Description,
            ErrorType.Problem => error.Description,
            ErrorType.NotFound => error.Description,
            ErrorType.Conflict => error.Description,
            _ => "An error occurred while processing the request."
        };

        static string GetType(Error error) => error.Type switch
        {
            ErrorType.Failure => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        static int GetStatusCode(Error error) => error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Problem => StatusCodes.Status500InternalServerError,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        static string GetInstance() => $"{_contextAccessor?.HttpContext?.Request.Method} {_contextAccessor?.HttpContext?.Request.Path}";

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            if (result.Error is not ValidationError validationError)
            {
                return null;
            }

            return new Dictionary<string, object?>
            {
                {"errors", validationError.Errors}
            };

        };

    }


}