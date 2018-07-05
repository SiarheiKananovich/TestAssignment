using System.Collections.Generic;

namespace TvMazeScraper.BusinessLogic.Interface.Models
{
	public class ShowModel
	{
		public string Name { get; set; }

		public IEnumerable<CastModel> Casts { get; set; }
	}
}