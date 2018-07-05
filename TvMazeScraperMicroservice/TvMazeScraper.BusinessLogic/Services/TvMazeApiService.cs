using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TvMazeScraper.BusinessLogic.DataModels;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class TvMazeApiService : ITvMazeApiService
	{
		private readonly IOptions<TvMazeApiConfig> _apiConfig;
		private readonly IMapper _mapper;


		public TvMazeApiService(IOptions<TvMazeApiConfig> apiConfig, IMapper mapper)
		{
			_apiConfig = apiConfig;
			_mapper = mapper;
		}


		public async Task<IEnumerable<int>> GetTvMazeShowsIdsAsync()
		{
			var requestUrl = _apiConfig.Value.ShowsSearchApiUrl;
			IEnumerable<int> tvMazeShowsIds = null;

			try
			{
				var client = new HttpClient();
				var response = await client.GetStringAsync(requestUrl);
				var tvMazeSearchData = JsonConvert.DeserializeObject<IEnumerable<TvMazeSearchIdOnlyData>>(response);
				tvMazeShowsIds = tvMazeSearchData.Select(data => data.Show.Id);
			}
			catch (HttpRequestException exception)
			{
				//todo
				throw;
			}
			catch (JsonReaderException exception)
			{
				//todo
				throw;
			}

			return tvMazeShowsIds;
		}

		public async Task<TvMazeShowModel> GetTvMazeShowAsync(int id)
		{
			var requestUrl = _apiConfig.Value.ShowInfoApiUrl;
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
				//todo
				throw;
			}
			catch (JsonReaderException exception)
			{
				//todo
				throw;
			}

			return _mapper.Map<TvMazeShowModel>(tvMazeShow);
		}

		public async Task<IEnumerable<TvMazeCastModel>> GetTvMazeCastsAsync(int id)
		{
			var requestUrl = _apiConfig.Value.ShowCastsApiUrl;
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
				//todo
				throw;
			}
			catch (JsonReaderException exception)
			{
				//todo
				throw;
			}

			return _mapper.Map<IEnumerable<TvMazeCastModel>>(tvMazePersons);
		}
	}
}