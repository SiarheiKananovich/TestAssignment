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
			var shows = await _database
				.Shows
				.Include(show => show.Casts)
				.AsNoTracking()
				.OrderBy(show => show.Name)
				.Skip(skip)
				.Take(take)
				.ToListAsync();

			foreach (var show in shows)
			{
				if (show?.Casts != null)
				{
					show.Casts = show.Casts.OrderByDescending(cast => cast.Birthday ?? DateTime.MaxValue).ToList();
				}
			}

			//// The same query but with big impact on database performance
			//var query = from showItem in _database.Shows
			//			join castItem in 
			//				(from cast in _database.Casts
			//				orderby cast.Birthday.HasValue descending, cast.Birthday
			//				select cast) on showItem.Id equals castItem.Id
			//			select showItem;

			return shows;
		}

		public async Task<Show> GetShowAsync(int id)
		{
			var show = await _database
				.Shows
				.Include(showItem => showItem.Casts)
				.AsNoTracking()
				.OrderBy(showItem => showItem.Name)
				.SingleOrDefaultAsync(showItem => showItem.Id == id);

			if (show?.Casts != null)
			{
				show.Casts = show.Casts.OrderByDescending(cast => cast.Birthday ?? DateTime.MaxValue).ToList();
			}

			//// The same query but with big impact on database performance
			//var query = from showItem in _database.Shows
			//			join castItem in 
			//				(from cast in _database.Casts
			//				orderby cast.Birthday.HasValue descending, cast.Birthday
			//				select cast) on showItem.Id equals castItem.Id
			//			select showItem;

			return show;
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

		public async Task<bool> IsTvMazeShowImportedAsync(int tvMazeShowId)
		{
			var externalShowMap = await _database
				.ExternalShowMap
				.AsNoTracking()
				.SingleOrDefaultAsync(map => map.TvMazeShowId == tvMazeShowId);

			if (externalShowMap == null)
			{
				return false;
			}

			return true;
		}

		public async Task AddTvMazeShowAsync(int tvMazeShowId, Show show)
		{
			if (tvMazeShowId == 0 || show == null)
			{
				throw new ArgumentException($"{nameof(tvMazeShowId)} {nameof(show)}");
			}

			var showMap = new ExternalShowMap
			{
				Show = show,
				TvMazeShowId = tvMazeShowId
			};

			try
			{
				_database.ExternalShowMap.Add(showMap);
				await _database.SaveChangesAsync();
			}
			catch (DbUpdateException exception)
			{
				throw new DatabaseException("Database update error occured.", exception);
			}
		}
	}
}