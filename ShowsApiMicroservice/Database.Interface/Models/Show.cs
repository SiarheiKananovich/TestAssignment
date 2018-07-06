using System.Collections.Generic;

namespace Database.Interface.Models
{
	public class Show
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<Cast> Casts { get; set; }
	}
}