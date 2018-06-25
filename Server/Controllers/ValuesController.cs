using System;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		// GET api/v1/values
		[HttpGet]
		public JsonResult Get()
		{
			var a = new Show
			{
				Id = 1,
				Casts = new []
				{
					new Cast {Id = 1, Name = "Mike", Birthday = DateTime.Today},
					new Cast {Id = 2, Name = "Simona", Birthday = DateTime.MinValue}
				}
			};

			return new JsonResult(a);
		}

		// GET api/v1/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		// POST api/v1/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/v1/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/v1/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
