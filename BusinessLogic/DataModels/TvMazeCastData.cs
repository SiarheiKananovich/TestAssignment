using Newtonsoft.Json;

namespace BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeCastData
	{
		[JsonProperty(PropertyName = "person")]
		public TvMazePersonData Person { get; set; }
	}
}