﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeScraper.BusinessLogic.DataModels
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TvMazeShowData
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		public IEnumerable<TvMazePersonData> Casts { get; set; }
	}
}