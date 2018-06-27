using System;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Server.Middleware
{
	public class SilentExceptionHandler
	{
		private const string DEFAULT_ERROR_MESSAGE = "An internal error occurred";
		private readonly RequestDelegate _next;
		private readonly ILogger<SilentExceptionHandler> _logger;

		public SilentExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_logger = loggerFactory?.CreateLogger<SilentExceptionHandler>() 
					  ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (BusinessLogicException exception)
			{
				// TODO add strings to constants
				_logger.LogError(exception, "A BusinessLogicException exception occurred.");

				if (context.Response.HasStarted)
				{
					_logger.LogWarning("The response has already started, the exception middleware will not be executed.");
					throw;
				}

				context.Response.Clear();
				context.Response.StatusCode = 500;
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(exception.Message);

				return;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "An unhandled exception occurred.");

				if (context.Response.HasStarted)
				{
					_logger.LogWarning("The response has already started, the exception middleware will not be executed.");
					throw;
				}

				context.Response.Clear();
				context.Response.StatusCode = 500;
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(DEFAULT_ERROR_MESSAGE);

				return;
			}
		}
	}

	public static class SilentExceptionHandlerMiddlewareExtensions
	{
		public static IApplicationBuilder UseSilentExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<SilentExceptionHandler>();
		}
	}
}