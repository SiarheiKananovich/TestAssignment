using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Database;

namespace BusinessLogic
{
	public static class BusinessLogicConfigurator
	{
		public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
		{
			DatabaseConfigurator.ConfigureServices(configuration, services);

			services.AddSingleton<IMapper, Mapper>();
			services.AddTransient<IShowsService, ShowsService>();
			services.AddTransient<ITvMazeShowsService, TvMazeShowsService>();
		}
	}
}