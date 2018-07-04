using System;
using Newtonsoft.Json;

namespace ApiServer.Models
{
	public class ApiCast
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "birthday")]
		public DateTime? Birthday { get; set; }
	}
}