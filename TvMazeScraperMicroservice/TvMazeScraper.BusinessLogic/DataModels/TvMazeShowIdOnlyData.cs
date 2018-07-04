using Newtonsoft.Json;

namespace TvMazeScraper.BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeShowIdOnlyData
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
	}
}