using System;
using System.Collections.Generic;
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
	public class ShowService
	{
		private readonly DatabaseContext _database;
		private readonly IMapper _mapper;

		public ShowService(DatabaseContext database, IMapper mapper)
		{
			_database = database;
		}

		public async Task<IEnumerable<ApiShow>> GetShowsAsync(int skip, int take)
		{
			try
			{
				var shows = await _database
					.Shows
					.OrderBy(show => show.Name)
					.Skip(skip)
					.Take(take)
					.ToListAsync();

				return _mapper.MapCollection<Show, ApiShow>(shows);
			}
			catch (Exception exception)
			{
				throw new ServiceException("An error occurred while executing the database query.", exception);
			}
		}
	}
}