using System.Collections.Generic;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Interface.Interfaces
{
	public interface IMicroservicesCommunicationService
	{
		void SendImportResults(IEnumerable<ImportResultModel> importResults);
	}
}