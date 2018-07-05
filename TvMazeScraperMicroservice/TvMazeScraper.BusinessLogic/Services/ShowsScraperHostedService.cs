using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class ShowsScraperHostedService : IHostedService, IDisposable
	{
		private readonly IOptions<AppConfig> _appConfig;
		private readonly ILogger<ShowsScraperHostedService> _logger;
		private readonly ITvMazeScraperService _tvMazeScraperService;

		private Timer _timer;


		public ShowsScraperHostedService(IOptions<AppConfig> appConfig, ILogger<ShowsScraperHostedService> logger, ITvMazeScraperService tvMazeScraperService)
		{
			_appConfig = appConfig;
			_logger = logger;
			_tvMazeScraperService = tvMazeScraperService;
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			var period = TimeSpan.FromMinutes(_appConfig.Value.ScraperUpdatePeriodInMinutes);

			_timer = new Timer(ImportTvMazeShows, null, TimeSpan.Zero, period);

			return Task.CompletedTask;
		}

		private async void ImportTvMazeShows(object state)
		{
			await _tvMazeScraperService.ImportNewTvMazeShowsAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
