using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

		private IModel _showsImportChannel;


		public MicroservicesCommunicationService(IOptions<CommunicationConfig> config)
		{
			_config = config.Value;

			OpenChannels();
		}


		public void EnqueueImportResults(IEnumerable<ImportResultModel> importResults)
		{
			var data = SerializeData(importResults);
			_showsImportChannel.BasicPublish(_config.ImportResultsExchange, String.Empty, null, data);
		}

		public void RegisterImportRequestsConsumer(Func<IEnumerable<ImportRequestModel>, Task> handler)
		{
			var showsImportConsumer = new EventingBasicConsumer(_showsImportChannel);
			showsImportConsumer.Received += (sender, eventArgs) =>
			{
				var importRequests = DeserializeData<IEnumerable<ImportRequestModel>>(eventArgs.Body, true);
				handler(importRequests);
			};
			_showsImportChannel.BasicConsume(queue: _config.ImportRequestsQueue, autoAck: true, consumer: showsImportConsumer);
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