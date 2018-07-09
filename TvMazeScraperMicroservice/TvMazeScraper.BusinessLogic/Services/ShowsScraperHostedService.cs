using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.Infrastructure.Interface;
using TvMazeScraper.Infrastructure.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class ShowsScraperHostedService : IHostedService
	{
		private readonly ILogger<ShowsScraperHostedService> _logger;
		private readonly ITvMazeScraperService _tvMazeScraperService;
		private readonly IStringsProvider _strings;


		public ShowsScraperHostedService(
			ILogger<ShowsScraperHostedService> logger, 
			ITvMazeScraperService tvMazeScraperService,
			IStringsProvider strings)
		{
			_logger = logger;
			_tvMazeScraperService = tvMazeScraperService;
			_strings = strings;
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_strings[StringsEnum.SCRAPER_HOSTED_SERVICE_STARTED]);

			_tvMazeScraperService.StartImportNewTvMazeShowsAsync(cancellationToken);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_strings[StringsEnum.SCRAPER_HOSTED_SERVICE_STOPED]);

			return Task.CompletedTask;
		}
	}
}
