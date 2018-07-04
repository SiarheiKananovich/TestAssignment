using System.Net;

namespace ApiServer.Models
{
	public class ApiError
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
	}
}