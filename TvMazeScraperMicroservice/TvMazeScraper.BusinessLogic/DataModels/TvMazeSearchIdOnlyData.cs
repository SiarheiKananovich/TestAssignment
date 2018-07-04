using Newtonsoft.Json;

namespace TvMazeScraper.BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeSearchIdOnlyData
	{
		[JsonProperty(PropertyName = "show")]
		public TvMazeShowIdOnlyData Show { get; set; }
	}
}