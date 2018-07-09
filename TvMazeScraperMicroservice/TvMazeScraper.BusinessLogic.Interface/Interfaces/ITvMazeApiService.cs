using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface ITvMazeApiService
	{
		Task<IEnumerable<int>> GetTvMazeShowsIdsAsync(int skip, int take);
		Task<TvMazeShowModel> GetTvMazeShowAsync(int id);
		Task<IEnumerable<TvMazeCastModel>> GetTvMazeCastsAsync(int id);
	}
}