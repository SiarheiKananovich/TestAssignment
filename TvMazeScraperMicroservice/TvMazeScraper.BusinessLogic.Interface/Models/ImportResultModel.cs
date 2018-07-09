using System;

namespace TvMazeScraper.BusinessLogic.Interface.Models
{
	public struct ImportResultModel
	{
		public Guid RequestId { get; set; }

		public bool Success { get; set; }

		public int ShowId { get; set; }
	}
}