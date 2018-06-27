using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Database.Exceptions;
using Database.Interfaces;
using Database.Models;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace BusinessLogic.Services
{
	public class ShowsService : IShowsService
	{
		private readonly IDatabase _database;
		private readonly IMapper _mapper;
		private readonly ILogger<ShowsService> _logger;


		public ShowsService(IDatabase database, IMapper mapper, ILoggerFactory loggerFactory)
		{
			_database = database;
			_mapper = mapper;
			_logger = loggerFactory.CreateLogger<ShowsService>();
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
				return (null, new ApiError {StatusCode = HttpStatusCode.NotFound, Message = String.Format(Defines.Error.SHOW_DOES_NOT_EXIST, id)});
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
			catch (DatabaseException exception)
			{
				_logger.LogError(exception, String.Format(Defines.ErrorLog.INTERNAL_ERROR, exception.Message));
				return new ApiError { StatusCode = HttpStatusCode.InternalServerError, Message = String.Format(Defines.Error.FAILED_TO_ADD_SHOW, apiShow.Name) };
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
					_logger.LogWarning(Defines.ErrorLog.INVALID_PARAMS);
					return new ApiError { StatusCode = HttpStatusCode.NotFound, Message = String.Format(Defines.Error.SHOW_DOES_NOT_EXIST, id) };
				}
			}
			catch (DatabaseException exception)
			{
				_logger.LogError(exception, String.Format(Defines.ErrorLog.INTERNAL_ERROR, exception.Message));
				return new ApiError { StatusCode = HttpStatusCode.InternalServerError, Message = String.Format(Defines.Error.FAILED_TO_DELETE_SHOW, id) };
			}

			return null;
		}


		private bool ValidateGetShowParams(int skip, int take, out ApiError error)
		{
			error = null;

			if (skip < 0 || take <= 0)
			{
				error = new ApiError { StatusCode = HttpStatusCode.BadRequest, Message = Defines.Error.INVALID_SKIP_TAKE_PARAMS };
			}

			if (error != null)
			{
				_logger.LogWarning(Defines.ErrorLog.INVALID_PARAMS);
				return false;
			}

			return true;
		}

		private bool ValidateAddShowParams(Show show, out ApiError error)
		{
			error = null;

			if (String.IsNullOrEmpty(show.Name))
			{
				error = new ApiError { StatusCode = HttpStatusCode.BadRequest, Message = Defines.Error.INVALID_SHOW_NAME_PARAM_EMPTY };
			}

			if (error != null)
			{
				_logger.LogWarning(Defines.ErrorLog.INVALID_PARAMS);
				return false;
			}

			return true;
		}
	}
}