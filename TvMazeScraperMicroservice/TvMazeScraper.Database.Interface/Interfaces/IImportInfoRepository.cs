using System.Threading.Tasks;
using TvMazeScraper.Database.Interface.Models;

namespace TvMazeScraper.Database.Interface.Interfaces
{
	public interface IImportInfoRepository
	{
		Task<bool> IsImportInfoExistForTvMazeShowAsync(int tvMazeShowId);

		Task AddImportInfoAsync(ImportInfo importInfo);
	}
}