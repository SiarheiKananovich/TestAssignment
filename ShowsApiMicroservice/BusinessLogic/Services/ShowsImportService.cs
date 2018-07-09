using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Interface.Models;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
	public class ShowsImportService : IShowsImportService
	{
		private readonly ILogger<ShowsImportService> _logger;
		private readonly IStringsProvider _strings;
		private readonly IMicroservicesCommunicationService _communicationService;
		private readonly IShowsService _showsService;


		public ShowsImportService(
			ILogger<ShowsImportService> logger,
			IStringsProvider strings,
			IMicroservicesCommunicationService communicationService, 
			IShowsService showsService)
		{
			_logger = logger;
			_strings = strings;
			_communicationService = communicationService;
			_showsService = showsService;
		}


		public void StartProcessImportRequests()
		{
			_logger.LogInformation(_strings[StringsEnum.START_PROCESS_IMPORT_REQUESTS]);

			_communicationService.RegisterImportRequestsConsumers(ImportAsync);
		}

		public async Task ImportAsync(IEnumerable<ImportRequestModel> importRequests)
		{
			importRequests = importRequests.ToList();
			var shows = importRequests.Select(request => request.Show).ToList();
			var importResults = new ImportResultModel[importRequests.Count()];

			// Try adding shows within a single transaction
			if (await _showsService.AddShowsAsync(shows))
			{
				_logger.LogInformation(_strings[StringsEnum.START_SHOW_IMPORT_SINGLE_BLOCK]);

				int index = 0;
				foreach (var request in importRequests)
				{
					importResults[index] = new ImportResultModel(request.Id, true, shows[index].Id);
					++index;
				}
			}
			// If failed, try adding one by one
			else
			{
				_logger.LogInformation(_strings[StringsEnum.FAILED_SHOW_IMPORT_SINGLE_BLOCK]);
				_logger.LogInformation(_strings[StringsEnum.START_SHOW_IMPORT_ONE_BY_ONE]);

				int index = 0;
				foreach (var request in importRequests)
				{
					var importSuccess = await _showsService.AddShowAsync(request.Show);
					importResults[index] = new ImportResultModel(request.Id, importSuccess, request.Show.Id);
					++index;
				}
			}

			_communicationService.EnqueueImportResults(importResults);
		}
	}
}