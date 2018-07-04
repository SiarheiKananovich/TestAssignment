using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class ShowsScraperHostedService : IHostedService, IDisposable
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger<ShowsScraperHostedService> _logger;
		private readonly ITvMazeScraperService _tvMazeShowsApiService;

		private Timer _timer;


		public ShowsScraperHostedService(IConfiguration configuration, ILoggerFactory loggerFactory, ITvMazeScraperService tvMazeShowsApiService)
		{
			_configuration = configuration;
			_logger = loggerFactory.CreateLogger<ShowsScraperHostedService>();
			_tvMazeShowsApiService = tvMazeShowsApiService;
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			var period = 5;//double.Parse(_configuration[Defines.Config.TVMAZE_SCRAPPER_UPDATE_PERIOD]);

			_timer = new Timer(ImportTvMazeShows, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(period));

			return Task.CompletedTask;
		}

		private async void ImportTvMazeShows(object state)
		{
			var tvMazeShows = await _tvMazeShowsApiService.GetTvMazeShowsAsync();

			foreach (var show in tvMazeShows)
			{
				if (!await _tvMazeShowsApiService.IsTvMazeShowImportedAsync(show.Id))
				{
					await _tvMazeShowsApiService.ImportTvMazeShowAsync(show);
				}
			}
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
