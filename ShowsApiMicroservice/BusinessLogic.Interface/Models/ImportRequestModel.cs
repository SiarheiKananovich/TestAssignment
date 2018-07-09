using System;

namespace BusinessLogic.Interface.Models
{
	public class ImportRequestModel
	{
		public Guid Id { get; set; }

		public ShowModel Show { get; set; }
	}
}