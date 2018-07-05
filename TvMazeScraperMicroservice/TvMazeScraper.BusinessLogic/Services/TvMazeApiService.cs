using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
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
		private readonly IHttpService _httpService;


		public TvMazeApiService(
			IOptions<TvMazeApiConfig> apiConfig, 
			IMapper mapper, 
			IHttpService httpService)
		{
			_apiConfig = apiConfig;
			_mapper = mapper;
			_httpService = httpService;
		}


		public async Task<IEnumerable<int>> GetTvMazeShowsIdsAsync()
		{
			var requestUrl = _apiConfig.Value.ShowsSearchApiUrl;

			var tvMazeSearchData = await _httpService.GetAsync<IEnumerable<TvMazeSearchIdOnlyData>>(requestUrl);
			var tvMazeShowsIds = tvMazeSearchData.Select(data => data.Show.Id);

			return tvMazeShowsIds;
		}

		public async Task<TvMazeShowModel> GetTvMazeShowAsync(int id)
		{
			var requestUrl = _apiConfig.Value.ShowInfoApiUrl;
			requestUrl = String.Format(requestUrl, id);

			var tvMazeShow = await _httpService.GetAsync<TvMazeShowData>(requestUrl);

			return _mapper.Map<TvMazeShowModel>(tvMazeShow);
		}

		public async Task<IEnumerable<TvMazeCastModel>> GetTvMazeCastsAsync(int id)
		{
			var requestUrl = _apiConfig.Value.ShowCastsApiUrl;
			requestUrl = String.Format(requestUrl, id);

			var tvMazeSearchData = await _httpService.GetAsync<IEnumerable<TvMazeCastData>>(requestUrl);
			var tvMazePersons = tvMazeSearchData.Select(data => data.Person).ToList();

			return _mapper.Map<IEnumerable<TvMazeCastModel>>(tvMazePersons);
		}
	}
}