using Database.Interface.Interfaces;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
	public static class DatabaseConfigureExtension
	{
		private const string ConnectionStringKey = "DefaultConnection";


		public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString(ConnectionStringKey);
			services.AddDbContextPool<DatabaseContext>(options => options.UseSqlServer(connectionString));

			services.AddTransient<IShowRepository, ShowRepository>();

			return services;
		}
	}
}