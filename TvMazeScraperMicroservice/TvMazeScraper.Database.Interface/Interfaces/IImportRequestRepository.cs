using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Database.Interface.Models;

namespace TvMazeScraper.Database.Interface.Interfaces
{
	public interface IImportRequestRepository
	{
		Task<ImportRequest> GetAsync(Guid id);
		Task AddAsync(ImportRequest importRequest);
		Task AddManyAsync(IEnumerable<ImportRequest> importRequests);
		Task UpdateStatusAsync(Guid id, ImportRequestStatus newStatus);
		Task DeleteAsync(Guid id);
	}
}