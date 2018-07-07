using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TvMazeScraper.BusinessLogic.Interface.Configs;
using TvMazeScraper.BusinessLogic.Interface.Interfaces;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Services
{
	public class MicroservicesCommunicationService
	{
		private readonly CommunicationConfig _config;
		private readonly ITvMazeScraperService _tvMazeScraperService;

		private IModel _showsImportChannel;


		public MicroservicesCommunicationService(IOptions<CommunicationConfig> config, ITvMazeScraperService tvMazeScraperService)
		{
			_config = config.Value;
			_tvMazeScraperService = tvMazeScraperService;

			OpenChannels();
			RegisterConsumers();
		}


		public void SendShowsImportRequest(IEnumerable<ImportRequestModel> importRequests)
		{
			var data = SerializeData(importRequests);
			_showsImportChannel.BasicPublish("TestTvMazeScraperExchangeName", "RoutingKeyName", null, data);
		}


		private void OpenChannels()
		{
			var showsApiConnection = GetConnection(_config.UserName, _config.Password, _config.VirtualHost, _config.HostName);

			_showsImportChannel = showsApiConnection.CreateModel();
			_showsImportChannel.ExchangeDeclare("TestTvMazeScraperExchangeName", ExchangeType.Direct);
			_showsImportChannel.QueueDeclare("TestQueueName", false, false, false, null);
			_showsImportChannel.QueueBind("TestQueueName", "TestExchangeName", "RoutingKeyName", null);
		}

		private void RegisterConsumers()
		{
			var processShowsImportResultsConsumer = new AsyncEventingBasicConsumer(_showsImportChannel);
			processShowsImportResultsConsumer.Received += async (_, args) =>
			{
				var importResults = DeserializeData<IEnumerable<ImportResultModel>>(args.Body);
				await _tvMazeScraperService.ProcessImportResultsAsync(importResults);
			};

			_showsImportChannel.BasicConsume(queue: "TestQueueName", autoAck: true, consumer: processShowsImportResultsConsumer);
		}

		private static IConnection GetConnection(string userName, string password, string virtualHost, string hostName)
		{
			var factory = new ConnectionFactory
			{
				UserName = userName,
				Password = password,
				VirtualHost = virtualHost,
				HostName = hostName
			};

			return factory.CreateConnection();
		}


		private static byte[] SerializeData<T>(T data)
		{
			using (var memoryStream = new MemoryStream())
			using (var writer = new BsonDataWriter(memoryStream))
			{
				var serializer = new JsonSerializer();
				serializer.Serialize(writer, data);

				return memoryStream.ToArray();
			}
		}

		private static T DeserializeData<T>(byte[] data)
		{
			using (var ms = new MemoryStream(data))
			using (var reader = new BsonDataReader(ms))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<T>(reader);
			}
		}
	}
}