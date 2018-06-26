using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessLogic.DataModels;
using BusinessLogic.Interfaces;
using Database.Interfaces;
using Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Models;

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


		public async Task<IEnumerable<ApiShow>> GetShowsAsync(string query)
		{
			string requestUrl = _configuration["TvMazeApi:ShowsSearchUrl"];
			requestUrl = String.Format(requestUrl, query);
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
				_logger.LogError(exception, "TvMazeApi not available. Error message: {0}", exception.Message);
				throw;
			}
			catch (JsonReaderException exception)
			{
				_logger.LogError(exception, "Invalid data received for TvMazeApi request. Error message: {0}", exception.Message);
				throw;
			}

			return _mapper.MapCollection<TvMazeShowData, ApiShow>(tvMazeShows);
		}

		public async Task<bool> ImportTvMazeShowAsync(int id)
		{
			var tvMazeShow = await GetTvMazeShowFromApiAsync(id);
			tvMazeShow.Casts = await GetTvMazeCastsFromApiAsync(id);

			Show show = _mapper.Map<TvMazeShowData, Show>(tvMazeShow);

			return IsShowModelValidForImport(show) && await TryAddNewShowAsync(show);
		}


		private async Task<TvMazeShowData> GetTvMazeShowFromApiAsync(int id)
		{
			string requestUrl = _configuration["TvMazeApi:ShowInfo"];
			requestUrl = String.Format(requestUrl, id);
			TvMazeShowData tvMazeShow = null;

			try
			{
				var client = new HttpClient();
				var response = await client.GetStringAsync(requestUrl);
				tvMazeShow = JsonConvert.DeserializeObject<TvMazeShowData>(response);
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

			return tvMazeShow;
		}

		private async Task<IEnumerable<TvMazerPerson>> GetTvMazeCastsFromApiAsync(int id)
		{
			string requestUrl = _configuration["TvMazeApi:ShowCasts"];
			requestUrl = String.Format(requestUrl, id);
			IEnumerable<TvMazerPerson> tvMazePersons = null;

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

			return tvMazePersons;
		}

		private bool IsShowModelValidForImport(Show show)
		{
			return String.IsNullOrWhiteSpace(show.Name) == false;
		}

		private async Task<bool> TryAddNewShowAsync(Show show)
		{
			return await _database.ShowRepository.AddShowAsync(show);
		}
	}
}