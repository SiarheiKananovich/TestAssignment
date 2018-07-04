using System.Threading.Tasks;
using MongoDB.Driver;
using TvMazeScraper.Database.Interface.Interfaces;
using TvMazeScraper.Database.Interface.Models;

namespace TvMazeScraper.Database.Repositories
{
	public class ImportInfoRepository : IImportInfoRepository
	{
		private readonly DatabaseContext _database;


		public ImportInfoRepository(DatabaseContext database)
		{
			_database = database;
		}


		public async Task<bool> IsImportInfoExistForTvMazeShowAsync(int tvMazeShowId)
		{
			var result = await _database.ImportInfoCollection
				.FindAsync(new ExpressionFilterDefinition<ImportInfo>(item => item.ImportedTvMazeShowId == tvMazeShowId),
						   new FindOptions<ImportInfo> {Limit = 1});

			return await result.AnyAsync();
		}

		public Task AddImportInfoAsync(ImportInfo importInfo)
		{
			return _database.ImportInfoCollection
				.InsertOneAsync(importInfo);
		}
	}
}