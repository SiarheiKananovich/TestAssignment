using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interface.Exceptions;
using Database.Interface.Interfaces;
using Database.Interface.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
	public class ShowRepository : IShowRepository
	{
		private readonly DatabaseContext _database;

		public ShowRepository(DatabaseContext database)
		{
			_database = database;
		}

		public async Task<IEnumerable<Show>> GetShowsAsync(int skip, int take)
		{
			var shows = await _database
				.Shows
				.Include(show => show.Casts)
				.AsNoTracking()
				.OrderBy(show => show.Name)
				.Skip(skip)
				.Take(take)
				.ToListAsync();

			return shows;
		}

		public async Task AddShowAsync(Show show)
		{
			show.Id = 0;
			if (show.Casts != null)
			{
				foreach (var cast in show.Casts)
				{
					cast.Id = 0;
				}
			}

			try
			{
				_database.Shows.Add(show);
				await _database.SaveChangesAsync();
			}
			catch (DbUpdateException exception)
			{
				throw new DatabaseException("Database update error occured.", exception);
			}
		}
	}
}