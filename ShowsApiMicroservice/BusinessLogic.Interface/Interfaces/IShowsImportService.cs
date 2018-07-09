using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Interface.Interfaces
{
	public interface IShowsImportService
	{
		void StartProcessImportRequests();
		Task ImportAsync(IEnumerable<ImportRequestModel> importRequests);
	}
}