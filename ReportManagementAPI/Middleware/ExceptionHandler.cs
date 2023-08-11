using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using ReportManagementAPI.Models;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Middleware;

public  class ExceptionHandler
{
    private ILogManager _log;
    private RequestDelegate _next;

    public ExceptionHandler(ILogManager log, RequestDelegate next)
    {
        _log = log;
        _next = next;
    }


    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            CustomExceptionHandler(httpContext, e);
        }
    }

    public async void CustomExceptionHandler(HttpContext context, Exception exception)
    {
        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

        context.Response.Clear();
        context.Response.ContentType = "application/json";

        if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ApiErrorResponse(validationEx.Message));
            return;
        }

        if (exception is BadHttpRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ApiErrorResponse(exception.Message));
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var errorMsg = exception.Message;

        _log.LogError(new ExceptionLogDto()
        {
            Exception = exception,
            DateTime = DateTime.UtcNow,
            Source = "ClubForumApi"
        });


        var env = context.RequestServices.GetService<IWebHostEnvironment>();
        if (env.IsDevelopment())
        {
            errorMsg += $"\nStackTrace: {exception.StackTrace}";
        }

        await context.Response.WriteAsJsonAsync(new ApiErrorResponse(errorMsg));
    }
}