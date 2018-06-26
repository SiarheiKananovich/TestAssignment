using Database.Interfaces;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Database
{
	public static class DatabaseConfigurator
	{
		public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

			services.TryAddTransient<IShowRepository, ShowRepository>();
			services.TryAddTransient<IDatabase, Database>();
		}
	}
}