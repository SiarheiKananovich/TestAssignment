using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
	public static class DatabaseConfigurator
	{
		public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
		}
	}
}