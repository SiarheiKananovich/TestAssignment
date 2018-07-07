using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Interface.Models;

namespace BusinessLogic.Interface.Interfaces
{
	public interface IShowsImportService
	{
		Task ImportAsync(IEnumerable<ImportRequestModel> importRequests);
	}
}