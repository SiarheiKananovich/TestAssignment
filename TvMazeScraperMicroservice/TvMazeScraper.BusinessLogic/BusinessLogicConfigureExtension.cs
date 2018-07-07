using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Services;

namespace TvMazeScraper.BusinessLogic
{
	public static class BusinessLogicConfigureExtension
	{
		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration config)
		{
			services
				.Configure<AppConfig>(config.GetSection("App"))
				.Configure<TvMazeApiConfig>(config.GetSection("TvMazeApi"))
				.Configure<ShowsApiConfig>(config.GetSection("ShowsApi"))
				.Configure<CommunicationConfig>(config.GetSection("Communication"));

			services
				.AddSingleton<IHttpService, HttpService>()
				.AddSingleton<ITvMazeScraperService, TvMazeScraperService>()
				.AddTransient<ITvMazeApiService, TvMazeApiService>()
				.AddTransient<IShowsApiService, ShowsApiService>();

			services.AddHostedService<ShowsScraperHostedService>();

			return services;
		}
	}
}