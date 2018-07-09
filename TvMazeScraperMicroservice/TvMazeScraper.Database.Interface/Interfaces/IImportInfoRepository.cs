using System.Threading.Tasks;
using TvMazeScraper.Database.Interface.Models;

namespace TvMazeScraper.Database.Interface.Interfaces
{
	public interface IImportInfoRepository
	{
		Task<bool> IsImportInfoExistForTvMazeShowAsync(int tvMazeShowId);

		Task AddAsync(ImportInfo importInfo);
	}
}