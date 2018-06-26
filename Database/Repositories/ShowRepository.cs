using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Exceptions;
using Database.Interfaces;
using Database.Models;
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

				return shows.ToList();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public async Task<Show> GetShowAsync(int id)
		{
			var show = await _database
				.Shows
				.AsNoTracking()
				.SingleOrDefaultAsync(item => item.Id == id);

			return show;
		}

		public async Task<bool> AddShowAsync(Show show)
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
			catch (Exception exception)
			{

				return false;
			}

			return true;
		}

		public async Task<bool> DeleteShowAsync(int id)
		{
			var show = await _database
				.Shows
				.AsNoTracking()
				.SingleOrDefaultAsync(item => item.Id == id);

			if (show == null)
			{
				return false;
			}

			try
			{
				_database.Shows.Remove(show);
				await _database.SaveChangesAsync();
			}
			catch (DbUpdateException exception)
			{
				throw new DatabaseException("Database update error occured.", exception);
			}

			return true;
		}
	}
}