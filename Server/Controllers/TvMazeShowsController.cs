using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<JsonResult> Get([FromQuery][Required]string query)
		{
			var shows = await _tvMazeShowsService.GetShowsAsync(query);

			return new JsonResult(shows);
		}

		// POST api/v1/tvmaze/shows/import
		[Route("import")]
		[HttpPost]
		public async Task Import([FromBody][Required]int id)
		{
			await _tvMazeShowsService.ImportTvMazeShowAsync(id);
		}
	}
}