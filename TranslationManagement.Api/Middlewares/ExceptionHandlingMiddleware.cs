using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TranslationManagement.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public GlobalExceptionHandlingMiddleware(RequestDelegate next)
	{
		_next = Guard.Against.Null(next);
	}

	public Task InvokeAsync(HttpContext context)
	{
		try
		{
			return _next(context);
		}
		catch (Exception e)
		{
			//logger.LogCritical("eee");
			return HandleException(context, e);
		}
	}

	private static Task HandleException(HttpContext httpContext, Exception exception)
	{
		var statusCode = GetStatusCode();
		var response = new
		{
			title = GetTitle(),
			status = statusCode,
			detail = exception.Message,
			errors = GetErrors(exception)
		};

		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = statusCode;

		return httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
	}

	private static string GetTitle() => "Server Error";

	private static int GetStatusCode() => StatusCodes.Status500InternalServerError;


	private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
	{
		//TODO: implement getting errors from exception
		IReadOnlyDictionary<string, string[]> errors = new Dictionary<string, string[]>();

		return errors;
	}
}