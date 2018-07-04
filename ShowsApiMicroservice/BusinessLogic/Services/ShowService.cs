using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Interface.Models;
using BusinessLogic.Interfaces;
using Database.Interface.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
	public class ShowsService : IShowsService
	{
		private readonly IShowRepository _showRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<ShowsService> _logger;


		public ShowsService(IMapper mapper, ILogger<ShowsService> logger, IShowRepository showRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_showRepository = showRepository;
		}


		public async Task<IEnumerable<ShowModel>> GetShowsAsync(int skip, int take)
		{
			if (skip < 0 || take <= 0)
			{
				throw new ArgumentException("todo");
			}

			var shows = await _showRepository.GetShowsAsync(skip, take);

			foreach (var show in shows)
			{
				if (show?.Casts != null)
				{
					show.Casts = show.Casts.OrderByDescending(cast => cast.Birthday ?? DateTime.MaxValue);
				}
			}

			return _mapper.Map<IEnumerable<ShowModel>>(shows);
		}
	}
}