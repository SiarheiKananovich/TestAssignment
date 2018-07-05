using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using ApiServer.Models;
using AutoMapper;
using BusinessLogic.Interface.Interfaces;
using BusinessLogic.Interface.Models;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiServer.Controllers
{
	[Route("api/v1/shows")]
	[ApiController]
	public class ShowsController : ControllerBase
	{
		private readonly IShowsService _showService;
		private readonly IMapper _mapper;
		private readonly IStringsProvider _strings;
		private readonly ILogger<ShowsController> _logger;


		public ShowsController(IMapper mapper, IShowsService showService, IStringsProvider strings, ILogger<ShowsController> logger)
		{
			_mapper = mapper;
			_showService = showService;
			_strings = strings;
			_logger = logger;
		}

		// GET api/v1/shows?skip=2&take=3
		[HttpGet]
		public async Task<ActionResult> Get([FromQuery][Required]int skip, [FromQuery][Required]int take)
		{
			if (skip < 0 || take <= 0)
			{
				_logger.LogWarning(_strings[StringsEnum.WARNING_SAMPLE]);
				return StatusCode((int)HttpStatusCode.BadRequest, _strings[StringsEnum.ARGUMENT_ERROR_SKIP_TAKE]);
			}

			var shows = await _showService.GetShowsAsync(skip, take);

			var apiShows = _mapper.Map<IEnumerable<ApiShow>>(shows);

			return new JsonResult(apiShows);
		}

		// TODO: Add authorization or separate to ShowsImportMicroservice if public access is not needed, or use RabbitMQ RPC
		// PUT api/v1/shows
		[HttpPut]
		public async Task<ActionResult> Put([FromBody][Required]ApiShow apiShow)
		{
			ShowModel show;
			try
			{
				show = _mapper.Map<ShowModel>(apiShow);
			}
			catch (AutoMapperMappingException exception)
			{
				_logger.LogWarning(exception, _strings[StringsEnum.WARNING_SAMPLE]);
				return StatusCode((int)HttpStatusCode.BadRequest, _strings[StringsEnum.API_INVALID_SHOW_MODEL_ERROR]);
			}

			var result = await _showService.AddShowAsync(show);

			return new JsonResult(result);
		}
	}
}
