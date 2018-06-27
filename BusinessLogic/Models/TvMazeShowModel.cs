using System.Collections.Generic;

namespace BusinessLogic.Models
{
	public class TvMazeShowModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<TvMazeCastModel> Casts { get; set; }
	}
}