using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.Infrastructure.Interface;
using TvMazeScraper.Infrastructure.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class ShowsScraperHostedService : IHostedService, IDisposable
	{
		private readonly IOptions<AppConfig> _appConfig;
		private readonly ILogger<ShowsScraperHostedService> _logger;
		private readonly ITvMazeScraperService _tvMazeScraperService;
		private readonly IStringsProvider _strings;

		private Timer _timer;


		public ShowsScraperHostedService(
			IOptions<AppConfig> appConfig, 
			ILogger<ShowsScraperHostedService> logger, 
			ITvMazeScraperService tvMazeScraperService,
			IStringsProvider strings)
		{
			_appConfig = appConfig;
			_logger = logger;
			_tvMazeScraperService = tvMazeScraperService;
			_strings = strings;
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_strings[StringsEnum.SCRAPER_HOSTED_SERVICE_STARTING]);

			var period = TimeSpan.FromMinutes(_appConfig.Value.ScraperUpdatePeriodInMinutes);

			_timer = new Timer(ImportTvMazeShows, null, TimeSpan.Zero, period);

			_logger.LogInformation(_strings[StringsEnum.SCRAPER_HOSTED_SERVICE_STARTED]);

			return Task.CompletedTask;
		}

		private async void ImportTvMazeShows(object state)
		{
			_logger.LogInformation(_strings[StringsEnum.SCRAPER_HOSTED_SERVICE_IMPORT_STARTED]);

			await _tvMazeScraperService.ImportNewTvMazeShowsAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);

			_logger.LogInformation(_strings[StringsEnum.SCRAPER_HOSTED_SERVICE_STOPED]);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
