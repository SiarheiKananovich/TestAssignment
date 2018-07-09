using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TvMazeScraper.Database.Interface.Interfaces;
using TvMazeScraper.Database.Interface.Models;

namespace TvMazeScraper.Database.Repositories
{
	public class ImportRequestRepository : IImportRequestRepository
	{
		private readonly DatabaseContext _database;


		public ImportRequestRepository(DatabaseContext database)
		{
			_database = database;
		}


		public async Task<ImportRequest> GetAsync(Guid id)
		{
			var filter = new ExpressionFilterDefinition<ImportRequest>(item => item.Id == id);
			var cursor = await _database.ImportRequestCollection.FindAsync(filter);

			return await cursor.SingleAsync();
		}

		public Task AddAsync(ImportRequest importRequest)
		{
			return _database.ImportRequestCollection.InsertOneAsync(importRequest);
		}

		public Task AddManyAsync(IEnumerable<ImportRequest> importRequests)
		{
			return _database.ImportRequestCollection.InsertManyAsync(importRequests);
		}

		public async Task UpdateStatusAsync(Guid id, ImportRequestStatus newStatus)
		{
			var filter = new ExpressionFilterDefinition<ImportRequest>(item => item.Id == id);
			var update = Builders<ImportRequest>.Update.Set(item => item.Status, newStatus);
			await _database.ImportRequestCollection.UpdateOneAsync(filter, update);
		}

		public Task DeleteAsync(Guid id)
		{
			var filter = new ExpressionFilterDefinition<ImportRequest>(item => item.Id == id);
			return _database.ImportRequestCollection.DeleteOneAsync(filter);
		}
	}
}