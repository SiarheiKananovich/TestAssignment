using Newtonsoft.Json;

namespace BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeSearchData
	{
		[JsonProperty(PropertyName = "score")]
		public int Score { get; set; }

		[JsonProperty(PropertyName = "show")]
		public TvMazeShowData Show { get; set; }
	}
}