using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace BusinessLogic.Interfaces
{
	public interface IShowsService
	{
		Task<IEnumerable<ApiShow>> GetShowsAsync(int skip, int take);
	}
}