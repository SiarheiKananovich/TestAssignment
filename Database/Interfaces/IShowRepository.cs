using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Models;

namespace Database.Interfaces
{
	public interface IShowRepository
	{
		Task<IEnumerable<Show>> GetShowsAsync(int skip, int take);
		Task<Show> GetShowAsync(int id);
		Task<bool> AddShowAsync(Show show);
		Task<bool> DeleteShowAsync(int id);
	}
}