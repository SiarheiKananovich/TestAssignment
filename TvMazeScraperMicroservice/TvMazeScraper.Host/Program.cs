using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using TvMazeScraper.BusinessLogic;
using TvMazeScraper.Database;
using TvMazeScraper.Infrastructure;

namespace TvMazeScraper.Host
{
	public class Program
	{
		private const string APP_SETTINGS_FILE_NAME = "appsettings.json";
		private const string STRINGS_FILE_NAME = "strings.json";
		private const string CONFIG_LOGGING_SECTION_NAME = "Logging";

		public static async Task Main(string[] args)
		{
			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile(APP_SETTINGS_FILE_NAME)
						.AddJsonFile(STRINGS_FILE_NAME)
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
						.ConfigureInfrastructureServices()
						.ConfigureDatabaseServices(hostContext.Configuration)
						.ConfigureBusinessLogicServices(hostContext.Configuration);
				})
				.ConfigureLogging((hostingContext, logging) => {
					logging.AddConfiguration(hostingContext.Configuration.GetSection(CONFIG_LOGGING_SECTION_NAME));
					logging.AddConsole();
				});

			await builder.RunConsoleAsync();
		}
	}
}
