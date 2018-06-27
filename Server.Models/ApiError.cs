using System.Net;

namespace Server.Models
{
	public class ApiError
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
	}
}