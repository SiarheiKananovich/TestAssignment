using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Services;

namespace TvMazeScraper.BusinessLogic
{
	public static class BusinessLogicConfigureExtension
	{
		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.Configure<AppConfig>(configuration.GetSection("App"))
				.Configure<TvMazeApiConfig>(configuration.GetSection("TvMazeApi"));

			services
				.AddTransient<ITvMazeScraperService, TvMazeScraperService>()
				.AddTransient<ITvMazeApiService, TvMazeApiService>();

			return services;
		}
	}
}