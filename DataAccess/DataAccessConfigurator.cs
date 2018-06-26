using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataAccess
{
    public static class DataAccessConfigurator
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
