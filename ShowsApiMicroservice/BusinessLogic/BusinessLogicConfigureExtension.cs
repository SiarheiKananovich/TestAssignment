using BusinessLogic.Interface.Configs;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
	public static class BusinessLogicConfigureExtension
	{
		private const string CommunicationConfigSection = "Communication";


		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<CommunicationConfig>(config.GetSection(CommunicationConfigSection));

			services
				.AddSingleton<IMicroservicesCommunicationService, MicroservicesCommunicationService>()
				.AddTransient<IShowsImportService, ShowsImportService>()
				.AddTransient<IShowsService, ShowsService>();

			services.AddHostedService<MicroservicesCommunicationHostedService>();

			return services;
		}
	}
}