using System;
using System.Collections.Generic;
using System.Net;
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


		public async Task<(IEnumerable<ApiShow>, ApiError)> GetShowsAsync(int skip, int take)
		{
			if (!ValidateGetShowParams(skip, take, out var error))
			{
				return (null, error);
			}

			var shows = await _database.ShowRepository.GetShowsAsync(skip, take);

			return (_mapper.MapCollection<Show, ApiShow>(shows), null);
		}

		public async Task<(ApiShow, ApiError)> GetShowAsync(int id)
		{
			var show = await _database.ShowRepository.GetShowAsync(id);

			if (show == null)
			{
				return (null, new ApiError {StatusCode = HttpStatusCode.NotFound, Message = $"Show with id={id} does not exist."});
			}

			return (_mapper.Map<Show, ApiShow>(show), null);
		}

		public async Task<ApiError> AddShowAsync(ApiShow apiShow)
		{
			var show = _mapper.Map<ApiShow, Show>(apiShow);

			if (!ValidateAddShowParams(show, out var error))
			{
				return error;
			}

			try
			{
				await _database.ShowRepository.AddShowAsync(show);
			}
			catch (DatabaseException)
			{
				return new ApiError { StatusCode = HttpStatusCode.InternalServerError, Message = $"Internal error: failed to add a '{apiShow.Name}' show." };
			}

			return null;
		}

		public async Task<ApiError> DeleteShowAsync(int id)
		{
			try
			{
				var deleted = await _database.ShowRepository.DeleteShowAsync(id);

				if (!deleted)
				{
					return new ApiError { StatusCode = HttpStatusCode.NotFound, Message = $"Show with id={id} does not exist."};
				}
			}
			catch (DatabaseException)
			{
				return new ApiError { StatusCode = HttpStatusCode.InternalServerError, Message = $"Internal error: failed to delete a show with id={id}." };
			}

			return null;
		}


		private bool ValidateGetShowParams(int skip, int take, out ApiError error)
		{
			if (skip < 0 || take <= 0)
			{
				error = new ApiError
					{
						StatusCode = HttpStatusCode.BadRequest,
						Message = $"Invalid params: use 'skip' < 0 and 'take' <= 0."
					};
				return false;
			}

			error = null;
			return true;
		}

		private bool ValidateAddShowParams(Show show, out ApiError error)
		{
			if (String.IsNullOrEmpty(show.Name))
			{
				error = new ApiError { StatusCode = HttpStatusCode.BadRequest, Message = "Invalid 'name' value: should be not null or empty." };
				return false;
			}

			error = null;
			return true;
		}
	}
}