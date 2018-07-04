using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Database.Interface.Configs;
using TvMazeScraper.Database.Interface.Interfaces;

namespace TvMazeScraper.Database
{
	public static class DatabaseConfigureExtension
	{
		public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.Configure<ConnectionStringsConfig>(configuration.GetSection("ConnectionStrings"));

			services
				.AddTransient<DatabaseContext>()
				.AddTransient<IImportInfoRepository, IImportInfoRepository>();

			return services;
		}
	}
}