using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic.Interface.Interfaces
{
	public interface IMicroservicesCommunicationService
	{
		void EnqueueShowsImportRequest(IEnumerable<ImportRequestModel> importRequests);
		void RegisterShowsImportResultsConsumer(Func<IEnumerable<ImportResultModel>, Task> handler);
	}
}