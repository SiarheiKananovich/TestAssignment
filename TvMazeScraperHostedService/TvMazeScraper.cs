using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TvMazeScraperHostedService
{
	public class TvMazeScraper : IHostedService, IDisposable
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger<TvMazeScraper> _logger;
		private readonly ITvMazeShowsService _tvMazeShowsService;

		private Timer _timer;


		public TvMazeScraper(IConfiguration configuration, ILoggerFactory loggerFactory, ITvMazeShowsService tvMazeShowsService)
		{
			_configuration = configuration;
			_logger = loggerFactory.CreateLogger<TvMazeScraper>();
			_tvMazeShowsService = tvMazeShowsService;
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Background Service is starting.");

			var period = double.Parse(_configuration[Defines.Config.TVMAZE_SCRAPPER_UPDATE_PERIOD]);

			_timer = new Timer(ImportTvMazeShows, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(period));

			return Task.CompletedTask;
		}

		private async void ImportTvMazeShows(object state)
		{
			_logger.LogInformation("Timed Background Service is working.");

			var tvMazeShows = await _tvMazeShowsService.GetTvMazeShowsAsync();

			foreach (var show in tvMazeShows)
			{
				if (!await _tvMazeShowsService.IsTvMazeShowImportedAsync(show.Id))
				{
					await _tvMazeShowsService.ImportTvMazeShowAsync(show);
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Background Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
