using System.Collections.Generic;

namespace TvMazeScraper.BusinessLogic.Interface.Models
{
	public class TvMazeShowModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<TvMazeCastModel> Casts { get; set; }
	}
}