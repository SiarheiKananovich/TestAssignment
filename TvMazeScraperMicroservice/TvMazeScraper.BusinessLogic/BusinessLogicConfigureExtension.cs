using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Services;

namespace TvMazeScraper.BusinessLogic
{
	public static class BusinessLogicConfigureExtension
	{
		private const string AppConfigSection = "App";
		private const string TvMazeApiConfigSection = "TvMazeApi";
		private const string ShowsApiConfigSection = "ShowsApi";
		private const string CommunicationConfigSection = "Communication";


		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddMemoryCache();

			services
				.Configure<AppConfig>(config.GetSection(AppConfigSection))
				.Configure<TvMazeApiConfig>(config.GetSection(TvMazeApiConfigSection))
				.Configure<ShowsApiConfig>(config.GetSection(ShowsApiConfigSection))
				.Configure<CommunicationConfig>(config.GetSection(CommunicationConfigSection));

			services
				.AddSingleton<IHttpService, HttpService>()
				.AddSingleton<ITvMazeScraperService, TvMazeScraperService>()
				.AddSingleton<ITvMazeApiService, TvMazeApiService>()
				.AddTransient<IMicroservicesCommunicationService, MicroservicesCommunicationService>();

			services.AddHostedService<ShowsScraperHostedService>();

			return services;
		}
	}
}