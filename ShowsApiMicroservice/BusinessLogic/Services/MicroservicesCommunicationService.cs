using System.Collections.Generic;
using System.IO;
using System.Text;
using BusinessLogic.Interface.Configs;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Interface.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLogic.Services
{
	public class MicroservicesCommunicationService : IMicroservicesCommunicationService
	{
		private readonly CommunicationConfig _config;
		private readonly IShowsImportService _showsImportService;

		private IModel _showsImportChannel;


		public MicroservicesCommunicationService(
			IOptions<CommunicationConfig> config,
			IShowsImportService showsImportService)
		{
			_config = config.Value;
			_showsImportService = showsImportService;

			OpenChannels();
			RegisterConsumers();
		}


		public void SendImportResults(IEnumerable<ImportResultModel> importResults)
		{
			var data = SerializeData(importResults);
			_showsImportChannel.BasicPublish("TestExchangeName", "RoutingKeyName", null, data);
		}


		private void OpenChannels()
		{
			var showsApiConnection = GetConnection(_config.UserName, _config.Password, _config.VirtualHost, _config.HostName);

			_showsImportChannel = showsApiConnection.CreateModel();
			_showsImportChannel.ExchangeDeclare("TestApiServerExchangeName", ExchangeType.Direct);
			_showsImportChannel.QueueDeclare("TestQueueName", false, false, false, null);
			_showsImportChannel.QueueBind("TestQueueName", "TestExchangeName", "RoutingKeyName", null);
		}

		private void RegisterConsumers()
		{
			var showsImportConsumer = new AsyncEventingBasicConsumer(_showsImportChannel);
			showsImportConsumer.Received += async (_, args) =>
			{
				var importRequests = DeserializeData<IEnumerable<ImportRequestModel>>(args.Body);
				await _showsImportService.ImportAsync(importRequests);
			};

			_showsImportChannel.BasicConsume(queue: "TestQueueName", autoAck: true, consumer: showsImportConsumer);
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