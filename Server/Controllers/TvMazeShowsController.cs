using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	[Route("api/v1/tvmaze/shows")]
    [ApiController]
    public class TvMazeShowsController : ControllerBase
	{
		private readonly ITvMazeShowsService _tvMazeShowsService;


		public TvMazeShowsController(ITvMazeShowsService tvMazeShowsService)
		{
			_tvMazeShowsService = tvMazeShowsService;
		}


		// GET api/v1/tvmaze/shows?query=game
		[HttpGet]
		public async Task<ActionResult> Get([FromQuery][Required]string query)
		{
			var (shows, error) = await _tvMazeShowsService.GetShowsAsync(query);

			if (error != null)
			{
				return GetApiErrorResponse(error);
			}

			return new JsonResult(shows);
		}

		// POST api/v1/tvmaze/shows/import
		[Route("import")]
		[HttpPost]
		public async Task<ActionResult> Import([FromBody][Required]int id)
		{
			var error = await _tvMazeShowsService.ImportTvMazeShowAsync(id);

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