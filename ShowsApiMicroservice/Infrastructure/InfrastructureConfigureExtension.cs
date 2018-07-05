using Infrastructure.Interface.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class InfrastructureConfigureExtension
	{
		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services)
		{
			services.AddTransient<IStringsProvider, StringsProvider>();

			return services;
		}
	}
}