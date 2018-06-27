using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
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
		public async Task<ActionResult> Get([FromQuery][Required]int skip, [FromQuery][Required]int take)
		{
			var (shows, error) = await _showService.GetShowsAsync(skip, take);

			if (error != null)
			{
				return GetApiErrorResponse(error);
			}

			return new JsonResult(shows);
		}

		// GET api/v1/shows/5
		[HttpGet("{id}")]
		public async Task<ActionResult> Get(int id)
		{
			var (show, error) = await _showService.GetShowAsync(id);

			if (error != null)
			{
				return GetApiErrorResponse(error);
			}

			return new JsonResult(show);
		}

		// Post api/v1/shows
		[HttpPost]
		public async Task<ActionResult> Put([FromBody] ApiShow show)
		{
			var error = await _showService.AddShowAsync(show);

			if (error != null)
			{
				return GetApiErrorResponse(error);
			}

			return Ok();
		}

		// DELETE api/v1/shows/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var error = await _showService.DeleteShowAsync(id);

			if (error != null)
			{
				return GetApiErrorResponse(error);
			}

			return Ok();
		}


		private ObjectResult GetApiErrorResponse(ApiError error)
		{
			return StatusCode((int)error.StatusCode, new
			{
				ErrorMessage = error.Message
			});
		}
	}
}
