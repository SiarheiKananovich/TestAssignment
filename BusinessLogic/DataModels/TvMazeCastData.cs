using Newtonsoft.Json;

namespace BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeCastData
	{
		[JsonProperty(PropertyName = "person")]
		public TvMazerPerson Person { get; set; }
	}
}