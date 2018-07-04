using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Services;

namespace TvMazeScraper.Host
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<ShowsScraperHostedService>();
				});

			await builder.RunConsoleAsync();
		}
	}
}
