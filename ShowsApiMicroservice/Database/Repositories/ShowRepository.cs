﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interface.Exceptions;
using Database.Interface.Interfaces;
using Database.Interface.Models;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
	public class ShowRepository : IShowRepository
	{
		private readonly DatabaseContext _database;
		private readonly IStringsProvider _strings;


		public ShowRepository(DatabaseContext database, IStringsProvider strings)
		{
			_database = database;
			_strings = strings;
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

		public async Task AddShowsAsync(IEnumerable<Show> shows)
		{
			using (var transaction = await _database.Database.BeginTransactionAsync())
			{
				try
				{
					foreach (var show in shows)
					{
						// prevent custom id generation
						show.Id = 0;
						if (show.Casts != null)
						{
							foreach (var cast in show.Casts)
							{
								cast.Id = 0;
							}
						}

						_database.Shows.Add(show);
					}

					await _database.SaveChangesAsync();
					transaction.Commit();
				}
				catch (DbUpdateException exception)
				{
					transaction.Rollback();
					throw new DatabaseException(_strings[StringsEnum.DATABASE_ERROR_SAMPLE], exception);
				}
			}
		}

		public async Task AddShowAsync(Show show)
		{
			// prevent custom id generation
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
				throw new DatabaseException(_strings[StringsEnum.DATABASE_ERROR_SAMPLE], exception);
			}
		}
	}
}