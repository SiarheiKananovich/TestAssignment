using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using Database.Exceptions;
using Database.Interfaces;
using Database.Models;
using Server.Models;

namespace BusinessLogic.Services
{
	public class ShowsService : IShowsService
	{
		private readonly IDatabase _database;
		private readonly IMapper _mapper;


		public ShowsService(IDatabase database, IMapper mapper)
		{
			_database = database;
			_mapper = mapper;
		}


		public async Task<IEnumerable<ApiShow>> GetShowsAsync(int skip, int take)
		{
			var shows = await _database.ShowRepository.GetShowsAsync(skip, take);

			return _mapper.MapCollection<Show, ApiShow>(shows);
		}

		public async Task<ApiShow> GetShowAsync(int id)
		{
			var show = await _database.ShowRepository.GetShowAsync(id);

			return _mapper.Map<Show, ApiShow>(show);
		}

		public async Task AddShowAsync(ApiShow apiShow)
		{
			var show = _mapper.Map<ApiShow, Show>(apiShow);

			if (!IsShowValidForAdding(show, out var errorMessage))
			{
				return;
			}

			try
			{
				await _database.ShowRepository.AddShowAsync(show);
			}
			catch (DatabaseException exception)
			{
				throw new BusinessLogicException($"Internal error: failed to add a '{apiShow.Name}' show.", exception);
			}
		}

		public async Task<bool> DeleteShowAsync(int id)
		{
			try
			{
				return await _database.ShowRepository.DeleteShowAsync(id);
			}
			catch (DatabaseException exception)
			{
				throw new BusinessLogicException($"Internal error: failed to delete show with id={id}.", exception);
			}
		}

		private bool IsShowValidForAdding(Show show, out string errorMessage)
		{
			errorMessage = null;

			if (String.IsNullOrEmpty(show.Name))
			{
				return false;
			}

			return true;
		}
	}
}