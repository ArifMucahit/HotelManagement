using System.ComponentModel.DataAnnotations;
using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace HotelManagementAPI.Middlewares;

public static class ExceptionHandler
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(err =>
        {
            err.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature.Error.GetBaseException();

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

                var env = context.RequestServices.GetService<IWebHostEnvironment>();
                if (env.IsDevelopment())
                {
                    errorMsg += $"\nStackTrace: {exception.StackTrace}";
                }

                await context.Response.WriteAsJsonAsync(new ApiErrorResponse(errorMsg));
            });
        });
    }
}