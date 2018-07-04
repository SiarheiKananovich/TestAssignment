using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Interfaces
{
	public interface IShowsService
	{
		Task<IEnumerable<ShowModel>> GetShowsAsync(int skip, int take);
	}
}