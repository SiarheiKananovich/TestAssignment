using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class HttpService : IHttpService
	{
		private const string JSON_MEDIA_TYPE = "application/json";


		public async Task<TResponse> GetAsync<TResponse>(string url)
		{
			var client = new HttpClient();
			var response = await client.GetStringAsync(url);

			return JsonConvert.DeserializeObject<TResponse>(response);
		}

		public async Task<TResponse> PutAsync<TResponse>(string url, object data)
		{
			var client = new HttpClient();
			HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, JSON_MEDIA_TYPE);
			var response = await client.PutAsync(url, content);

			return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync()); ;
		}
	}
}