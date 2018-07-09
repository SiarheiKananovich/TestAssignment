using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Interface.Models;

namespace Database.Interface.Interfaces
{
	public interface IShowRepository
	{
		Task<IEnumerable<Show>> GetShowsAsync(int skip, int take);
		Task<IEnumerable<int>> AddShowsAsync(IEnumerable<Show> shows);
		Task<int> AddShowAsync(Show show);
	}
}