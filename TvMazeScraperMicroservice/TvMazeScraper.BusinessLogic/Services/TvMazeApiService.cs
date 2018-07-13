using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TvMazeScraper.BusinessLogic.DataModels;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class TvMazeApiService : ITvMazeApiService
	{
		private const string TvMazePageCachePrefix = "TvMazePageCachePrefix";
		private const string TvMazeShowCachePrefix = "TvMazeShowCachePrefix";


		private readonly IOptions<TvMazeApiConfig> _apiConfig;
		private readonly IMapper _mapper;
		private readonly IHttpService _httpService;
		private readonly IMemoryCache _cache;


		public TvMazeApiService(
			IOptions<TvMazeApiConfig> apiConfig, 
			IMapper mapper, 
			IHttpService httpService,
			IMemoryCache cache)
		{
			_apiConfig = apiConfig;
			_mapper = mapper;
			_httpService = httpService;
			_cache = cache;
		}


		public async Task<IEnumerable<int>> GetTvMazeShowsIdsAsync(int page)
		{
			if (_cache.TryGetValue($"{TvMazePageCachePrefix}{page}", out IEnumerable<int> cachedPage))
			{
				return cachedPage;
			}

			var requestUrl = _apiConfig.Value.ShowsIndexApiUrl + page;
			IEnumerable<TvMazeShowData> tvMazePageShows;

			try
			{
				tvMazePageShows = await _httpService.GetAsync<IEnumerable<TvMazeShowData>>(requestUrl);
			}
			catch (HttpRequestException)
			{
				return null;
			}

			foreach (var show in tvMazePageShows)
			{
				_cache.Set($"{TvMazeShowCachePrefix}{show.Id}", show);
			}

			var tvMazePageShowsIds = tvMazePageShows.Select(data => data.Id);
			_cache.Set($"{TvMazePageCachePrefix}{page}", tvMazePageShowsIds);

			return tvMazePageShowsIds;
		}

		public async Task<TvMazeShowModel> GetTvMazeShowAsync(int id)
		{
			if (_cache.TryGetValue($"{TvMazeShowCachePrefix}{id}", out TvMazeShowData show))
			{
				return _mapper.Map<TvMazeShowModel>(show);
			}

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