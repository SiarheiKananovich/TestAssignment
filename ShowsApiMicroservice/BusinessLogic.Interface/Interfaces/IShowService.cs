using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Interface.Interfaces
{
	public interface IShowsService
	{
		Task<IEnumerable<ShowModel>> GetShowsAsync(int skip, int take);
		Task<bool> AddShowsAsync(IEnumerable<ShowModel> showModels);
		Task<bool> AddShowAsync(ShowModel show);
	}
}