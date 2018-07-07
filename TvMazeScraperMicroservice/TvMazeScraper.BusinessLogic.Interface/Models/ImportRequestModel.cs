using System;

namespace TvMazeScraper.BusinessLogic.Interface.Models
{
	public class ImportRequestModel
	{
		public Guid RequestId { get; set; }

		public ShowModel Show { get; set; }
	}
}