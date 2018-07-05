using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using TvMazeScraper.Database.Interface.Configs;
using TvMazeScraper.Database.Interface.Interfaces;
using TvMazeScraper.Database.Interface.Models;
using TvMazeScraper.Database.Repositories;

namespace TvMazeScraper.Database
{
	public static class DatabaseConfigureExtension
	{
		public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			RegisterDatabaseClassMaps();

			services
				.Configure<ConnectionStringsConfig>(configuration.GetSection("ConnectionStrings"));

			services
				.AddTransient<DatabaseContext>()
				.AddTransient<IImportInfoRepository, ImportInfoRepository>();

			return services;
		}

		private static void RegisterDatabaseClassMaps()
		{
			BsonClassMap.RegisterClassMap<ImportInfo>(cm =>
			{
				cm.AutoMap();
				cm.MapIdMember(c => c.Id).SetIgnoreIfDefault(true);
			});
		}
	}
}