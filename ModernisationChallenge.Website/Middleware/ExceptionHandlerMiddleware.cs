using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ModernisationChallenge.Website.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace AIAssets.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    private const string ContentType = "application/json";

    public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            ProblemDetails problemDetails = null;
            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status405MethodNotAllowed:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Method not allowed.",
                        Instance = context.Request.Path,
                        Detail = string.Empty,
                        Status = (int)HttpStatusCode.MethodNotAllowed,
                        Type = string.Empty
                    };
                    break;
            }
        }
        catch (Exception ex)
        {
            await ConvertToProblemDetails(context, ex);
        }
    }

    private async Task ConvertToProblemDetails(HttpContext context, Exception exception)
    {
        context.Response.ContentType = ContentType;

        ProblemDetails problemDetails = null;
        switch (exception)
        {
            case Exception ex:
                problemDetails = ex.ToProblemDetails(context);
                break;
        }

        if (_env.IsProduction() || _env.IsStaging())
        {
            problemDetails.Detail = problemDetails.Title;
        }

        await WriteResponseContentAsync(context, problemDetails);
    }

    private async Task WriteResponseContentAsync(HttpContext context, ProblemDetails problemDetails)
    {
        var jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var result = JsonConvert.SerializeObject(problemDetails, jsonSerializerSettings);
        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = ContentType;

        await context.Response.WriteAsync(result);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}