using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Interface.Interfaces
{
	public interface IMicroservicesCommunicationService
	{
		void RegisterImportRequestsConsumers(Func<IEnumerable<ImportRequestModel>, Task> handler);
		void EnqueueImportResults(IEnumerable<ImportResultModel> importResults);
	}
}