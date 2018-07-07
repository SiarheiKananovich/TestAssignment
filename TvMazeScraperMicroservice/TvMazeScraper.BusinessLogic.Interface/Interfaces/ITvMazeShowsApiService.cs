using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface ITvMazeScraperService
	{
		Task ImportNewTvMazeShowsAsync();
		Task ProcessImportResultsAsync(IEnumerable<ImportResultModel> importResults);
	}
}