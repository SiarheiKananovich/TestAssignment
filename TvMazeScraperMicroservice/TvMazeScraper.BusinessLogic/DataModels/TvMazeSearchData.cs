using Newtonsoft.Json;

namespace TvMazeScraper.BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeSearchData
	{
		[JsonProperty(PropertyName = "show")]
		public TvMazeShowData Show { get; set; }
	}
}