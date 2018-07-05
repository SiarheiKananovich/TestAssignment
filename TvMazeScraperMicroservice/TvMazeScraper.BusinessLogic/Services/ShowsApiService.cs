using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TvMazeScraper.BusinessLogic.DataModels;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Interface.Models;
using TvMazeScraper.Infrastructure.Interface;
using TvMazeScraper.Infrastructure.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class ShowsApiService : IShowsApiService
	{
		private readonly ILogger<ShowsApiService> _logger;
		private readonly IOptions<ShowsApiConfig> _apiConfig;
		private readonly IMapper _mapper;
		private readonly IHttpService _httpService;
		private readonly IStringsProvider _strings;


		public ShowsApiService(
			ILogger<ShowsApiService> logger,
			IOptions<ShowsApiConfig> apiConfig, 
			IMapper mapper, 
			IHttpService httpService,
			IStringsProvider strings)
		{
			_logger = logger;
			_apiConfig = apiConfig;
			_mapper = mapper;
			_httpService = httpService;
			_strings = strings;
		}


		public async Task<bool> TryImportShowAsync(ShowModel show)
		{
			var requestUrl = _apiConfig.Value.AddShowApiUrl;
			bool result;

			try
			{
				var showsData = _mapper.Map<ShowData>(show);
				
				result = await _httpService.PutAsync<bool>(requestUrl, showsData);
			}
			catch (HttpRequestException exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.ERROR_SAMPLE]);
				return false;
			}

			return result;
		}
	}
}