using System;
using Newtonsoft.Json;

namespace TvMazeScraper.BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CastData
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "birthday")]
		public DateTime? Birthday { get; set; }
	}
}