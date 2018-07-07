using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Services
{
	public class ShowsImportService : IShowsImportService
	{
		private readonly IMicroservicesCommunicationService _communicationService;
		private readonly IShowsService _showsService;


		public ShowsImportService(IMicroservicesCommunicationService communicationService, IShowsService showsService)
		{
			_communicationService = communicationService;
			_showsService = showsService;
		}


		public async Task ImportAsync(IEnumerable<ImportRequestModel> importRequests)
		{
			importRequests = importRequests.ToList();
			IEnumerable<ShowModel> shows = importRequests.Select(request => request.Show).ToList();
			ImportResultModel[] importResults;

			// Try adding shows within a single transaction
			if (await _showsService.AddShowsAsync(shows))
			{
				importResults = importRequests.Select(request => new ImportResultModel(request.RequestId, true)).ToArray();
			}
			// If failed, try adding one by one
			else
			{
				importResults = new ImportResultModel[importRequests.Count()]; ;

				int index = 0;
				foreach (var request in importRequests)
				{
					var importSuccess = await _showsService.AddShowAsync(request.Show);
					importResults[index] = new ImportResultModel(request.RequestId, importSuccess);
					++index;
				}
			}

			_communicationService.SendImportResults(importResults);
		}
	}
}