using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Interface.Models;
using TvMazeScraper.Database.Interface.Interfaces;
using TvMazeScraper.Database.Interface.Models;
using TvMazeScraper.Infrastructure.Interface;
using TvMazeScraper.Infrastructure.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class TvMazeScraperService : ITvMazeScraperService
	{
		private readonly ILogger<TvMazeScraperService> _logger;
		private readonly IMapper _mapper;
		private readonly ITvMazeApiService _tvMazeApiService;
		private readonly IImportInfoRepository _importInfoRepository;
		private readonly IShowsApiService _showsApiService;
		private readonly IStringsProvider _strings;


		public TvMazeScraperService(
			ILogger<TvMazeScraperService> logger, 
			IMapper mapper, 
			ITvMazeApiService tvMazeApiService,
			IImportInfoRepository importInfoRepository,
			IShowsApiService showsApiService,
			IStringsProvider strings)
		{
			_logger = logger;
			_mapper = mapper;
			_tvMazeApiService = tvMazeApiService;
			_importInfoRepository = importInfoRepository;
			_showsApiService = showsApiService;
			_strings = strings;
		}

		public async Task ImportNewTvMazeShowsAsync()
		{
			try
			{
				var tvMazeShowsIds = await _tvMazeApiService.GetTvMazeShowsIdsAsync();

				foreach (var tvMazeShowId in tvMazeShowsIds)
				{
					if (!await IsTvMazeShowImportedAsync(tvMazeShowId))
					{
						await ImportTvMazeShowAsync(tvMazeShowId);
					}
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.ERROR_SAMPLE]);
			}
		}

		private async Task ImportTvMazeShowAsync(int tvMazeShowId)
		{
			TvMazeShowModel tvMazeShow = await _tvMazeApiService.GetTvMazeShowAsync(tvMazeShowId);

			if (tvMazeShow != null)
			{
				tvMazeShow.Casts = await _tvMazeApiService.GetTvMazeCastsAsync(tvMazeShowId);
			}

			if (tvMazeShow?.Casts == null)
			{
				_logger.LogWarning(_strings[StringsEnum.INVALID_TVMAZE_DATA_RECEIVED]);
				return;
			}

			var show = _mapper.Map<ShowModel>(tvMazeShow);

			if (IsShowModelValidForImport(show) && await _showsApiService.TryImportShowAsync(show))
			{
				var importInfo = new ImportInfo
				{
					ImportedTvMazeShowId = tvMazeShowId
				};

				try
				{
					await _importInfoRepository.AddImportInfoAsync(importInfo);
				}
				catch (Exception exception)
				{
					_logger.LogCritical(exception, _strings[StringsEnum.ERROR_FAILED_ADD_IMPORT_INFO]);
					throw;
				}
			}
		}

		private Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId)
		{
			return _importInfoRepository.IsImportInfoExistForTvMazeShowAsync(tvMazeShowId);
		}

		private bool IsShowModelValidForImport(ShowModel show)
		{
			return String.IsNullOrWhiteSpace(show.Name) == false;
		}
	}
}