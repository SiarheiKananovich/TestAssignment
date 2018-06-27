using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
	public interface ITvMazeShowsService
	{
		Task<IEnumerable<TvMazeShowModel>> GetTvMazeShowsAsync();
		Task ImportTvMazeShowAsync(TvMazeShowModel tvMazeShow);
		Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId);
	}
}