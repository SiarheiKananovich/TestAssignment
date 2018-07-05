using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Interface.Models;
using Database.Interface.Exceptions;
using Database.Interface.Interfaces;
using Database.Interface.Models;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
	public class ShowsService : IShowsService
	{
		private readonly IShowRepository _showRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<ShowsService> _logger;
		private readonly IStringsProvider _strings;


		public ShowsService(
			IMapper mapper, 
			ILogger<ShowsService> logger, 
			IShowRepository showRepository, 
			IStringsProvider strings)
		{
			_mapper = mapper;
			_logger = logger;
			_showRepository = showRepository;
			_strings = strings;
		}


		public async Task<IEnumerable<ShowModel>> GetShowsAsync(int skip, int take)
		{
			if (skip < 0 || take <= 0)
			{
				throw new ArgumentException(_strings[StringsEnum.ARGUMENT_ERROR_SKIP_TAKE]);
			}

			IEnumerable<Show> shows;
			try
			{
				shows = await _showRepository.GetShowsAsync(skip, take);
			}
			catch (DatabaseException exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.DATABASE_ERROR_SAMPLE]);
				throw;
			}

			foreach (var show in shows)
			{
				if (show?.Casts != null)
				{
					show.Casts = show.Casts.OrderByDescending(cast => cast.Birthday ?? DateTime.MaxValue);
				}
			}

			return _mapper.Map<IEnumerable<ShowModel>>(shows);
		}

		public async Task<bool> AddShowAsync(ShowModel showModel)
		{
			try
			{
				var show = _mapper.Map<Show>(showModel);

				await _showRepository.AddShowAsync(show);
			}
			catch (DatabaseException exception)
			{
				_logger.LogError(exception, _strings[StringsEnum.DATABASE_ERROR_SAMPLE]);
				return false;
			}

			return true;
		}
	}
}