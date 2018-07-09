using System;

namespace TvMazeScraper.BusinessLogic.Interface.Models
{
	public class ImportRequestModel
	{
		public Guid Id { get; set; }

		public int TvMazeShowId { get; set; }

		public ShowModel Show { get; set; }


		public ImportRequestModel(int tvMazeShowId, ShowModel show)
		{
			Id = Guid.NewGuid();
			TvMazeShowId = tvMazeShowId;
			Show = show;
		}
	}
}