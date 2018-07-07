using System;

namespace BusinessLogic.Interface.Models
{
	public struct ImportResultModel
	{
		public Guid RequestId { get; set; }

		public bool Success { get; set; }


		public ImportResultModel(Guid requestId, bool success)
		{
			RequestId = requestId;
			Success = success;
		}
	}
}