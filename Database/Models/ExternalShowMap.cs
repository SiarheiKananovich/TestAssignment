using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
	public class ExternalShowMap
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int ShowId { get; set; }

		public Show Show { get; set; }

		public int TvMazeShowId { get; set; }
	}
}