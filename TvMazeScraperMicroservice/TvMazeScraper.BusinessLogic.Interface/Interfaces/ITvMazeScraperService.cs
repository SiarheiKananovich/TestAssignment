using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface ITvMazeScraperService
	{
		Task StartImportNewTvMazeShowsAsync(CancellationToken cancellationToken);
	}
}