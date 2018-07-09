using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Interface.Interfaces;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
	public class MicroservicesCommunicationHostedService : IHostedService
	{
		private readonly ILogger<MicroservicesCommunicationHostedService> _logger;
		private readonly IStringsProvider _strings;
		private readonly IShowsImportService _showsImportService;


		public MicroservicesCommunicationHostedService(
			IServiceProvider serviceProvider,
			ILogger<MicroservicesCommunicationHostedService> logger,
			IStringsProvider strings)
		{
			_logger = logger;
			_strings = strings;

			var serviceScope = serviceProvider.CreateScope();
			_showsImportService = serviceScope.ServiceProvider.GetService<IShowsImportService>();
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_strings[StringsEnum.COMMUNICATION_STARTED]);

			_showsImportService.StartProcessImportRequests();

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation(_strings[StringsEnum.COMMUNICATION_ENDED]);

			return Task.CompletedTask;
		}
	}
}