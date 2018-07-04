using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TvMazeScraper.BusinessLogic.DataModels;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Interface.Models;
using TvMazeScraper.Database.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class TvMazeScraperService : ITvMazeScraperService
	{
		private readonly ILogger<TvMazeScraperService> _logger;
		private readonly IMapper _mapper;
		private readonly ITvMazeApiService _tvMazeApiService;
		private readonly IImportInfoRepository _importInfoRepository;


		public TvMazeScraperService(
			ILogger<TvMazeScraperService> logger, 
			IMapper mapper, 
			ITvMazeApiService tvMazeApiService,
			IImportInfoRepository importInfoRepository)
		{
			_logger = logger;
			_mapper = mapper;
			_tvMazeApiService = tvMazeApiService;
			_importInfoRepository = importInfoRepository;
		}

		public async Task ImportTvMazeShowAsync(TvMazeShowModel tvMazeShow)
		{
			if (tvMazeShow != null)
			{
				tvMazeShow.Casts = await _tvMazeApiService.GetTvMazeCastsAsync(tvMazeShow.Id);
			}

			if (tvMazeShow?.Casts == null)
			{
				//todo
				return;
			}

			var show = _mapper.Map<ShowData>(tvMazeShow);

			try
			{
				if (IsShowModelValidForImport(show))
				{
					await _database.ShowRepository.AddTvMazeShowAsync(tvMazeShow.Id, show);
				}
			}
			catch (Exception exception)
			{
				//todo
				throw;
			}
		}

		public Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId)
		{
			return _importInfoRepository.IsImportInfoExistForTvMazeShowAsync(tvMazeShowId);
		}

		private bool IsShowModelValidForImport(ShowData show)
		{
			return String.IsNullOrWhiteSpace(show.Name) == false;
		}
	}
}