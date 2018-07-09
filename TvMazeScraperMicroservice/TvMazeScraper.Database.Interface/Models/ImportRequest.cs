using System;

namespace TvMazeScraper.Database.Interface.Models
{
	public enum ImportRequestStatus
	{
		PENDING,
		FAILED
	}

	public class ImportRequest
	{
		public Guid Id { get; set; }

		public int TvMazeShowId { get; set; }

		public ImportRequestStatus Status { get; set; } = ImportRequestStatus.PENDING;
	}
}