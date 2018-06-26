using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ApiShow
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "cast")]
		public IEnumerable<ApiCast> Casts { get; set; }
	}
}
