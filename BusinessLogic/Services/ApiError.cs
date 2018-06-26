using System.Net;

namespace BusinessLogic
{
	public class ApiError
	{
		public HttpStatusCode StatusCode { get; set; }
		public string message { get; set; }
	}
}