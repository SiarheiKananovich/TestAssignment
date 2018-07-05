using System;

namespace TvMazeScraper.Database.Interface.Models
{
	public class ImportInfo
	{
		public Guid Id { get; set; }

		public int ImportedTvMazeShowId { get; set; }

		public int RelatedShowId { get; set; }
	}
}