using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
	public class Cast
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int ShowId { get; set; }

		public Show Show { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		public DateTime? Birthday { get; set; }
	}
}