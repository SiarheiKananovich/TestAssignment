using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		// POST api/v1/shows
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/v1/shows/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/v1/shows/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
