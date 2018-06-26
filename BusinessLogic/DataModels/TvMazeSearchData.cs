using Newtonsoft.Json;

namespace BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeSearchData
	{
		[JsonProperty(PropertyName = "show")]
		public TvMazeShowData Show { get; set; }
	}
}