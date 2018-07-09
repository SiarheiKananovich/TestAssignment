using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using TvMazeScraper.Database.Interface.Configs;
using TvMazeScraper.Database.Interface.Interfaces;
using TvMazeScraper.Database.Interface.Models;
using TvMazeScraper.Database.Repositories;

namespace TvMazeScraper.Database
{
	public static class DatabaseConfigureExtension
	{
		private const string EnumStringConventionName = "EnumStringConvention";
		private const string ConnectionStringsConfigSection = "ConnectionStrings";


		public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			RegisterDatabaseClassMaps();

			services.Configure<ConnectionStringsConfig>(configuration.GetSection(ConnectionStringsConfigSection));

			services
				.AddTransient<DatabaseContext>()
				.AddTransient<IImportInfoRepository, ImportInfoRepository>()
				.AddTransient<IImportRequestRepository, ImportRequestRepository>();

			return services;
		}

		private static void RegisterDatabaseClassMaps()
		{
			var pack = new ConventionPack
			{
				new EnumRepresentationConvention(BsonType.String)
			};
			ConventionRegistry.Register(EnumStringConventionName, pack, type => true);

			BsonClassMap.RegisterClassMap<ImportInfo>(cm =>
			{
				cm.AutoMap();
				cm.MapIdMember(c => c.Id).SetIgnoreIfDefault(true);
			});
		}
	}
}