using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	[Route("api/v1/shows")]
	[ApiController]
	public class ShowsController : ControllerBase
	{
		private readonly IShowsService _showService;


		public ShowsController(IShowsService showService)
		{
			_showService = showService;
		}

		// GET api/v1/shows?skip=2&take=3
		[HttpGet]
		public async Task<JsonResult> Get([FromQuery][Required]int skip, [FromQuery][Required]int take)
		{
			var shows = await _showService.GetShowsAsync(skip, take);

			return new JsonResult(shows);
		}

		// GET api/v1/shows/5
		[HttpGet("{id}")]
		public async Task<JsonResult> Get(int id)
		{
			var show = await _showService.GetShowAsync(id);

			return new JsonResult(show);
		}

		// PUT api/v1/shows/5
		[HttpPut("{id}")]
		public async Task<JsonResult> Put(int id, [FromBody] ApiShow show)
		{
			show.Id = id;
			var result = await _showService.AddShowAsync(show);

			return new JsonResult(result);
		}

		// DELETE api/v1/shows/5
		[HttpDelete("{id}")]
		public async Task<JsonResult> Delete(int id)
		{
			var result = await _showService.DeleteShowAsync(id);

			return new JsonResult(result);
		}
	}
}
