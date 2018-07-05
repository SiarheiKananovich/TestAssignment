using System.Net.Http;
using System.Text;
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
	public class ShowsApiService : IShowsApiService
	{
		private readonly IOptions<ShowsApiConfig> _apiConfig;
		private readonly IMapper _mapper;


		public ShowsApiService(IOptions<ShowsApiConfig> apiConfig, IMapper mapper)
		{
			_apiConfig = apiConfig;
			_mapper = mapper;
		}


		public async Task<bool> TryImportShowAsync(ShowModel show)
		{
			var requestUrl = _apiConfig.Value.AddShowApiUrl;
			bool result;

			try
			{
				var showsData = _mapper.Map<ShowData>(show);

				var client = new HttpClient();
				HttpContent content = new StringContent(JsonConvert.SerializeObject(showsData), Encoding.UTF8, "application/json");
				var response = await client.PutAsync(requestUrl, content);
				result = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
			}
			catch (HttpRequestException)
			{
				//todo
				return false;
			}
			catch (JsonReaderException)
			{
				//todo
				return false;
			}

			return result;
		}
	}
}