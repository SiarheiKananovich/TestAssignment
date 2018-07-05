using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
	public static class BusinessLogicConfigureExtension
	{
		public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services)
		{
			services.AddTransient<IShowsService, ShowsService>();

			return services;
		}
	}
}