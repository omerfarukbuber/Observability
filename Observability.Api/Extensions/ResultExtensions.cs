using Observability.Api.Infrastructure;
using Observability.Shared;

namespace Observability.Api.Extensions;

internal static class ResultExtensions
{
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess
            ? onSuccess()
            : onFailure(result);
    }

    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result);
    }

    public static void ConfigureCustomResult(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        CustomResults.Configure(serviceScope.ServiceProvider.GetRequiredService<IHttpContextAccessor>());
    }
}