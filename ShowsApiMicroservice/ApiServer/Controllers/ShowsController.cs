using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApiServer.Models;
using AutoMapper;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
	[Route("api/v1/shows")]
	[ApiController]
	public class ShowsController : ControllerBase
	{
		private readonly IShowsService _showService;
		private readonly IMapper _mapper;


		public ShowsController(IMapper mapper, IShowsService showService)
		{
			_mapper = mapper;
			_showService = showService;
		}

		// GET api/v1/shows?skip=2&take=3
		[HttpGet]
		public async Task<ActionResult> Get([FromQuery][Required]int skip, [FromQuery][Required]int take)
		{
			var shows = await _showService.GetShowsAsync(skip, take);

			var apiShows = _mapper.Map<IEnumerable<ApiShow>>(shows);

			return new JsonResult(apiShows);
		}
	}
}
