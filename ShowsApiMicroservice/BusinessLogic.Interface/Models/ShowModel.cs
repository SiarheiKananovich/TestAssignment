using System.Collections.Generic;

namespace BusinessLogic.Interface.Models
{
	public class ShowModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<CastModel> Casts { get; set; }
	}
}