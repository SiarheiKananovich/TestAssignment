using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Infrastructure.Interface.Interfaces;
using TvMazeScraper.Infrastructure.Services;

namespace TvMazeScraper.Infrastructure
{
	public static class InfrastructureConfigureExtension
	{
		public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
		{
			services.AddSingleton<IStringsProvider, StringsProvider>();

			return services;
		}
	}
}