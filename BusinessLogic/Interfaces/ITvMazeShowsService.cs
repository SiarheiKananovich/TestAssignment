using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace BusinessLogic.Interfaces
{
	public interface ITvMazeShowsService
	{
		Task<(IEnumerable<ApiShow>, ApiError)> GetShowsAsync(string query);
		Task<ApiError> ImportTvMazeShowAsync(int id);
	}
}