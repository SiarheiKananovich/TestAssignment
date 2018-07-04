using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TvMazeScraper.Database.Interface.Configs;
using TvMazeScraper.Database.Interface.Models;

namespace TvMazeScraper.Database
{
	public class DatabaseContext
	{
		private readonly IMongoDatabase _database;

		public DatabaseContext(IOptions<ConnectionStringsConfig> connectionStringConfig)
		{
			var connectionString = connectionStringConfig.Value.DefaultConnection;

			var client = new MongoClient(connectionString);

			var connection = new MongoUrlBuilder(connectionString);
			_database = client.GetDatabase(connection.DatabaseName);
		}

		public IMongoCollection<ImportInfo> ImportInfoCollection => _database.GetCollection<ImportInfo>("ImportInfo");
	}
}