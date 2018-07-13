using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TvMazeScraper.BusinessLogic.Interface.Configs;
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
		private readonly CommunicationConfig _config;
		private readonly ILogger<TvMazeScraperService> _logger;
		private readonly IMapper _mapper;
		private readonly ITvMazeApiService _tvMazeApiService;
		private readonly IImportInfoRepository _importInfoRepository;
		private readonly IImportRequestRepository _importRequestRepository;
		private readonly IStringsProvider _strings;
		private readonly IMicroservicesCommunicationService _communicationService;

		private int _tvMazeShowsPage = 0;
		private int _tvMazeShowsSkipPageItems = 0;


		public TvMazeScraperService(
			IOptions<CommunicationConfig> appConfig,
			ILogger<TvMazeScraperService> logger, 
			IMapper mapper, 
			ITvMazeApiService tvMazeApiService,
			IImportInfoRepository importInfoRepository,
			IImportRequestRepository importRequestRepository,
			IStringsProvider strings,
			IMicroservicesCommunicationService communicationService)
		{
			_config = appConfig.Value;
			_logger = logger;
			_mapper = mapper;
			_tvMazeApiService = tvMazeApiService;
			_importInfoRepository = importInfoRepository;
			_importRequestRepository = importRequestRepository;
			_strings = strings;
			_communicationService = communicationService;
		}


		public async Task StartImportNewTvMazeShowsAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_strings[StringsEnum.STARTED_IMPORT_NEW_TVMAZE_SHOWS]);

			try
			{
				_communicationService.RegisterShowsImportResultsConsumer(ProcessImportResultsAsync);

				while (!cancellationToken.IsCancellationRequested)
				{
					var importRequests = await GetNextImportRequestsBatchAsync();

					if (importRequests == null)
					{
						return;
					}

					await AddPendingImportRequestsAsync(importRequests);

					_communicationService.EnqueueShowsImportRequest(importRequests);
				}
			}
			// todo: handle more specific exceptions
			catch (Exception exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.ERROR_SAMPLE]);
				throw;
			}
		}

		private async Task ProcessImportResultsAsync(IEnumerable<ImportResultModel> importResults)
		{
			_logger.LogInformation(_strings[StringsEnum.START_PROCESS_IMPORT_RESULTS]);

			try
			{
				foreach (var importResult in importResults)
				{
					if (importResult.Success)
					{
						await HandleSuccessedImportResultAsync(importResult);
					}
					else
					{
						await HandleFailedImportResultAsync(importResult);
					}
				}
			}
			// todo: handle more specific exceptions
			catch (Exception exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.ERROR_SAMPLE]);
				throw;
			}
		}

		private async Task<IEnumerable<ImportRequestModel>> GetNextImportRequestsBatchAsync()
		{
			var showsForImport = new List<ImportRequestModel>(_config.MaxBatchSize);

			do
			{
				var tvMazePageShows = await _tvMazeApiService.GetTvMazeShowsIdsAsync(_tvMazeShowsPage);

				// reached the last page 
				if (tvMazePageShows == null)
				{
					break;
				}

				tvMazePageShows = tvMazePageShows.Skip(_tvMazeShowsSkipPageItems);

				foreach (var tvMazeShowId in tvMazePageShows)
				{
					if (!await IsTvMazeShowImportedAsync(tvMazeShowId))
					{
						var showModel = await GetShowModelByTvMazeShowIdAsync(tvMazeShowId);

						if (IsShowModelValidForImport(showModel))
						{
							showsForImport.Add(new ImportRequestModel(tvMazeShowId, showModel));
						}
					}

					++_tvMazeShowsSkipPageItems;

					if (showsForImport.Count == _config.MaxBatchSize)
					{
						break;
					}
				}

				if (_tvMazeShowsSkipPageItems == tvMazePageShows.Count())
				{
					++_tvMazeShowsPage;
					_tvMazeShowsSkipPageItems = 0;
				}
			}
			while (showsForImport.Count < _config.MaxBatchSize);

			if (showsForImport.Any())
			{
				return showsForImport;
			}

			return null;
		}

		private async Task<ShowModel> GetShowModelByTvMazeShowIdAsync(int tvMazeShowId)
		{
			TvMazeShowModel tvMazeShow = await _tvMazeApiService.GetTvMazeShowAsync(tvMazeShowId);

			if (tvMazeShow != null)
			{
				tvMazeShow.Casts = await _tvMazeApiService.GetTvMazeCastsAsync(tvMazeShowId);
			}

			if (tvMazeShow?.Casts == null)
			{
				_logger.LogWarning(_strings[StringsEnum.INVALID_TVMAZE_DATA_RECEIVED]);
				return null;
			}

			return _mapper.Map<ShowModel>(tvMazeShow);
		}

		private Task AddPendingImportRequestsAsync(IEnumerable<ImportRequestModel> importRequestModels)
		{
			var importRequests = _mapper.Map<IEnumerable<ImportRequest>>(importRequestModels);

			return _importRequestRepository.AddManyAsync(importRequests);
		}

		private async Task HandleSuccessedImportResultAsync(ImportResultModel importResult)
		{
			var importRequest = await _importRequestRepository.GetAsync(importResult.RequestId);

			var importInfo = new ImportInfo
			{
				ImportedTvMazeShowId = importRequest.TvMazeShowId
			};

			await _importInfoRepository.AddAsync(importInfo);

			await _importRequestRepository.DeleteAsync(importResult.RequestId);
		}

		private async Task HandleFailedImportResultAsync(ImportResultModel importResult)
		{
			await _importRequestRepository.UpdateStatusAsync(importResult.RequestId, ImportRequestStatus.FAILED);
		}

		private Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId)
		{
			return _importInfoRepository.IsImportInfoExistForTvMazeShowAsync(tvMazeShowId);
		}

		private static bool IsShowModelValidForImport(ShowModel show)
		{
			return String.IsNullOrWhiteSpace(show.Name) == false;
		}
	}
}