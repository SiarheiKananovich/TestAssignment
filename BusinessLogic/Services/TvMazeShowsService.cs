﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessLogic.DataModels;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Database.Exceptions;
using Database.Interfaces;
using Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BusinessLogic.Services
{
	public class TvMazeShowsService : ITvMazeShowsService
	{
		private readonly IDatabase _database;
		private readonly IConfiguration _configuration;
		private readonly ILogger<TvMazeShowsService> _logger;
		private readonly IMapper _mapper;


		public TvMazeShowsService(IDatabase database, IConfiguration configuration, ILoggerFactory loggerFactory, IMapper mapper)
		{
			_database = database;
			_configuration = configuration;
			_logger = loggerFactory.CreateLogger<TvMazeShowsService>();
			_mapper = mapper;
		}


		public async Task<IEnumerable<TvMazeShowModel>> GetTvMazeShowsAsync()
		{
			string requestUrl = _configuration[Defines.Config.TVMAZE_API_SHOWS_SEARCH];
			IEnumerable<TvMazeShowData> tvMazeShows = null;

			try
			{
				var client = new HttpClient();
				var response = await client.GetStringAsync(requestUrl);
				var tvMazeSearchData = JsonConvert.DeserializeObject<IEnumerable<TvMazeSearchData>>(response);
				tvMazeShows = tvMazeSearchData.Select(data => data.Show).ToList();
			}
			catch (HttpRequestException exception)
			{
				_logger.LogError(exception, String.Format(Defines.ErrorLog.TVMAZE_API_NOT_AVAILABLE, exception.Message));
				throw new BusinessLogicException(Defines.Error.TVMAZE_API_NOT_AVAILABLE, exception);
			}
			catch (JsonReaderException exception)
			{
				_logger.LogError(exception, String.Format(Defines.ErrorLog.TVMAZE_API_NOT_AVAILABLE, exception.Message));
				throw new BusinessLogicException(Defines.Error.TVMAZE_API_NOT_AVAILABLE, exception);
			}

			return _mapper.MapCollection<TvMazeShowData, TvMazeShowModel>(tvMazeShows);
		}

		public async Task ImportTvMazeShowAsync(TvMazeShowModel tvMazeShow)
		{
			if (tvMazeShow != null)
			{
				tvMazeShow.Casts = await GetTvMazeCastsFromApiAsync(tvMazeShow.Id);
			}

			if (tvMazeShow?.Casts == null)
			{
				_logger.LogError(Defines.ErrorLog.TVMAZE_API_NOT_RECEIVED);
				throw new BusinessLogicException(Defines.Error.TVMAZE_API_NOT_RECEIVED);
			}

			Show show = _mapper.Map<TvMazeShowModel, Show>(tvMazeShow);

			PrepareShowForImport(show);

			try
			{
				if (IsShowModelValidForImport(show))
				{
					await _database.ShowRepository.AddTvMazeShowAsync(tvMazeShow.Id, show);
				}
			}
			catch (DatabaseException exception)
			{
				_logger.LogError(exception, Defines.ErrorLog.TVMAZE_SHOW_DATABASE_IMPORT_FAILED);
				throw new BusinessLogicException(Defines.Error.TVMAZE_SHOW_DATABASE_IMPORT_FAILED);
			}
		}

		public Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId)
		{
			return _database.ShowRepository.IsTvMazeShowImportedAsync(tvMazeShowId);
		}


		private async Task<IEnumerable<TvMazeCastModel>> GetTvMazeCastsFromApiAsync(int id)
		{
			string requestUrl = _configuration["TvMazeApi:ShowCasts"];
			requestUrl = String.Format(requestUrl, id);
			IEnumerable<TvMazePersonData> tvMazePersons = null;

			try
			{
				var client = new HttpClient();
				var response = await client.GetStringAsync(requestUrl);
				var tvMazeSearchData = JsonConvert.DeserializeObject<IEnumerable<TvMazeCastData>>(response);
				tvMazePersons = tvMazeSearchData.Select(data => data.Person).ToList();
			}
			catch (HttpRequestException exception)
			{
				_logger.LogError(exception, "TvMazeApi not available.");
				return null;
			}
			catch (JsonReaderException exception)
			{
				_logger.LogError(exception, "Invalid data received for TvMazeApi request.");
				return null;
			}

			return _mapper.MapCollection<TvMazePersonData, TvMazeCastModel>(tvMazePersons);
		}

		private void PrepareShowForImport(Show show)
		{
			show.Id = 0;

			foreach (var cast in show.Casts)
			{
				cast.Id = 0;
			}
		}

		private bool IsShowModelValidForImport(Show show)
		{
			return String.IsNullOrWhiteSpace(show.Name) == false;
		}
	}
}