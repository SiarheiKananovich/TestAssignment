using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ApiServer
{
    public class Program
	{
		private const string APP_SETTINGS_FILE_NAME = "appsettings.json";
		private const string STRINGS_FILE_NAME = "strings.json";


		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}


		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
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
				.UseStartup<Startup>();
    }
}
