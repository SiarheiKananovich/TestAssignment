using System.Collections.Generic;

namespace Server.Models
{
	public class Show
	{
		public int Id { get; set; }

		public IEnumerable<Cast> Casts { get; set; }
	}
}
