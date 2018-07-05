using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface IShowsApiService
	{
		Task<bool> TryImportShowAsync(ShowModel show);
	}
}