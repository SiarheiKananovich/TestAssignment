using System;
using System.Threading.Tasks;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ApiServer.Middleware
{
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;
		private readonly IStringsProvider _strings;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IStringsProvider strings)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_logger = logger;
			_strings = strings;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.ERROR_SAMPLE]);

				if (context.Response.HasStarted)
				{
					_logger.LogWarning(_strings[StringsEnum.WARNING_SAMPLE]);
					throw;
				}

				context.Response.Clear();
				context.Response.StatusCode = 500;
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(_strings[StringsEnum.ERROR_SAMPLE]);
			}
		}
	}

	public static class GlobalExceptionMiddlewareExtensions
	{
		public static IApplicationBuilder UseSilentExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<GlobalExceptionMiddleware>();
		}
	}
}