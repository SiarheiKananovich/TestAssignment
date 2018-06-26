using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace BusinessLogic.Interfaces
{
	public interface ITvMazeShowsService
	{
		Task<IEnumerable<ApiShow>> GetShowsAsync(string query);
		Task<bool> ImportTvMazeShowAsync(int id);
	}
}