using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace ModernisationChallenge.Website.Utils;

public static class ProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this Exception ex, HttpContext context)
    {
        return CreateProblemDetails(ex, context, HttpStatusCode.InternalServerError);
    }

    private static ProblemDetails CreateProblemDetails(this Exception ex, HttpContext context, HttpStatusCode httpStatusCode)
    {
        return new ProblemDetails
        {
            Title = GetExceptionTitle(ex),
            Detail = GetExceptionStackTrace(ex),
            Instance = context?.Request?.Path ?? string.Empty,
            Status = (int)httpStatusCode
        };
    }

    private static string GetExceptionTitle(Exception ex)
    {
        var title = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(ex?.Message))
        {
            title.AppendLine(ex.Message);
        }

        if (!string.IsNullOrWhiteSpace(ex?.InnerException?.Message))
        {
            title.AppendLine(ex.InnerException.Message);
        }

        return title.ToString();
    }

    private static string GetExceptionStackTrace(Exception ex)
    {
        var stackTrace = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(ex?.StackTrace))
        {
            stackTrace.AppendLine(ex.StackTrace);
        }

        if (!string.IsNullOrWhiteSpace(ex?.InnerException?.StackTrace))
        {
            stackTrace.AppendLine(ex.InnerException.StackTrace);
        }

        return stackTrace.ToString();
    }
}