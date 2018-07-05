using System.Threading.Tasks;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface ITvMazeScraperService
	{
		Task ImportNewTvMazeShowsAsync();
	}
}