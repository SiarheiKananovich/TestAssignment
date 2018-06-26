using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace BusinessLogic.Services
{
	public class ShowsService : IShowsService
	{
		private readonly DatabaseContext _database;
		private readonly IMapper _mapper;


		public ShowsService(DatabaseContext database, IMapper mapper)
		{
			_database = database;
			_mapper = mapper;
		}


		public async Task<IEnumerable<ApiShow>> GetShowsAsync(int skip, int take)
		{
			try
			{
				var shows = await _database
					.Shows
					.Include(show => show.Casts)
					.AsNoTracking()
					.OrderBy(show => show.Name)
					.Skip(skip)
					.Take(take)
					.ToListAsync();

				return _mapper.MapCollection<Show, ApiShow>(shows);
			}
			catch (DbException exception)
			{
				throw new ServiceException("An error occurred while executing the database query.", exception);
			}
		}

		public async Task<ApiShow> GetShowAsync(int id)
		{
			var show = await _database
				.Shows
				.AsNoTracking()
				.SingleOrDefaultAsync(item => item.Id == id);

			return _mapper.Map<Show, ApiShow>(show);
		}

		public async Task<bool> AddShowAsync(ApiShow apiSHow)
		{
			var show = _mapper.Map<ApiShow, Show>(apiSHow);

			show.Id = 0;
			_database.Shows.Add(show);
			await _database.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteShowAsync(int id)
		{
			var show = await _database
				.Shows
				.AsNoTracking()
				.SingleOrDefaultAsync(item => item.Id == id);

			_database.Shows.Remove(show);
			await _database.SaveChangesAsync();

			return true;
		}
	}
}