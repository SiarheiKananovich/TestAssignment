using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace BusinessLogic.Interfaces
{
	public interface IShowsService
	{
		Task<(IEnumerable<ApiShow>, ApiError)> GetShowsAsync(int skip, int take);
		Task<(ApiShow, ApiError)> GetShowAsync(int id);
		Task<ApiError> AddShowAsync(ApiShow apiShow);
		Task<ApiError> DeleteShowAsync(int id);
	}
}