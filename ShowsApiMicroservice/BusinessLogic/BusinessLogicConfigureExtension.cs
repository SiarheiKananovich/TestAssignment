using BusinessLogic.Interface.Configs;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
	public static class BusinessLogicConfigureExtension
	{
		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration config)
		{
			services
				.Configure<CommunicationConfig>(config.GetSection("Communication"));

			services
				.AddSingleton<IMicroservicesCommunicationService, MicroservicesCommunicationService>()
				.AddTransient<IShowsImportService, ShowsImportService>()
				.AddTransient<IShowsService, ShowsService>();

			return services;
		}
	}
}