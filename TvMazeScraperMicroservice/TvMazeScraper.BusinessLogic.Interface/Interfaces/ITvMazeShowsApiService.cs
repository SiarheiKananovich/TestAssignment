using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface ITvMazeScraperService
	{
		Task<IEnumerable<TvMazeShowModel>> GetTvMazeShowsAsync();
		Task ImportTvMazeShowAsync(TvMazeShowModel tvMazeShow);
		Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId);
	}
}