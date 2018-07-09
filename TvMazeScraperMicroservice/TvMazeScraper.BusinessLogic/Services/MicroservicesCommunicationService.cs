using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
	public class MicroservicesCommunicationService : IMicroservicesCommunicationService
	{
		private readonly CommunicationConfig _config;

		private IModel _showsImportChannel;


		public MicroservicesCommunicationService(IOptions<CommunicationConfig> config)
		{
			_config = config.Value;

			OpenChannels();
		}


		public void EnqueueShowsImportRequest(IEnumerable<ImportRequestModel> importRequests)
		{
			var data = SerializeData(importRequests);
			_showsImportChannel.BasicPublish(_config.ImportRequestsExchange, String.Empty, null, data);
		}

		public void RegisterShowsImportResultsConsumer(Func<IEnumerable<ImportResultModel>, Task> handler)
		{
			var showsImportResultsConsumer = new EventingBasicConsumer(_showsImportChannel);
			showsImportResultsConsumer.Received += (_, args) =>
			{
				var importResults = DeserializeData<IEnumerable<ImportResultModel>>(args.Body, true);
				handler(importResults);
			};

			_showsImportChannel.BasicConsume(queue: _config.ImportResultsQueue, autoAck: true, consumer: showsImportResultsConsumer);
		}


		private void OpenChannels()
		{
			var showsApiConnection = GetConnection(_config.UserName, _config.Password, _config.VirtualHost, _config.HostName);

			_showsImportChannel = showsApiConnection.CreateModel();
			_showsImportChannel.QueueDeclare(_config.ImportRequestsQueue, false, false, false, null);
			_showsImportChannel.ExchangeDeclare(_config.ImportRequestsExchange, ExchangeType.Direct);
			_showsImportChannel.QueueBind(_config.ImportRequestsQueue, _config.ImportRequestsExchange, String.Empty);

			_showsImportChannel.QueueDeclare(_config.ImportResultsQueue, false, false, false, null);
			_showsImportChannel.ExchangeDeclare(_config.ImportResultsExchange, ExchangeType.Direct);
			_showsImportChannel.QueueBind(_config.ImportResultsQueue, _config.ImportResultsExchange, String.Empty);
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

		private static T DeserializeData<T>(byte[] data, bool isArray = false)
		{
			using (var ms = new MemoryStream(data))
			using (var reader = new BsonDataReader(ms))
			{
				reader.ReadRootValueAsArray = isArray;

				var serializer = new JsonSerializer();
				return serializer.Deserialize<T>(reader);
			}
		}
	}
}