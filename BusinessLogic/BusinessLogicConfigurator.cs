using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
	public static class BusinessLogicConfigurator
	{
		public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
		{
			services.AddSingleton<IMapper, Mapper>();
			services.AddTransient<IShowsService, ShowsService>();
			services.AddTransient<ITvMazeShowsService, TvMazeShowsService>();
		}
	}
}