using System;

namespace Database.Interface.Models
{
	public class Cast
	{
		public int Id { get; set; }

		public int ShowId { get; set; }

		public Show Show { get; set; }

		public string Name { get; set; }

		public DateTime? Birthday { get; set; }
	}
}