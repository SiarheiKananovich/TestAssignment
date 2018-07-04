using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using TvMazeScraper.BusinessLogic;
using TvMazeScraper.Database;

namespace TvMazeScraper.Host
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config
						.AddJsonFile("appsettings.json")
						.AddEnvironmentVariables();

					if (args != null)
					{
						config.AddCommandLine(args);
					}
				})
				.ConfigureServices((hostContext, services) =>
				{
					services
						.AddAutoMapper()
						.AddOptions()
						.ConfigureDatabaseServices(hostContext.Configuration)
						.ConfigureBusinessLogicServices(hostContext.Configuration);

					//services.AddHostedService<ShowsScraperHostedService>();
				})
				.ConfigureLogging((hostingContext, logging) => {
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
				});

			await builder.RunConsoleAsync();
		}
	}
}
