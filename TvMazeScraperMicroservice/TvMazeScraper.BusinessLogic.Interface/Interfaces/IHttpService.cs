using System.Threading.Tasks;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface IHttpService
	{
		Task<TResponse> GetAsync<TResponse>(string url);

		Task<TResponse> PutAsync<TResponse>(string url, object data);
	}
}