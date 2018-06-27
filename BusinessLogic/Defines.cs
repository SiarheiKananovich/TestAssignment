namespace BusinessLogic
{
	public static class Defines
	{
		public static class Config
		{
			public const string TVMAZE_API_SHOWS_SEARCH = "TvMazeApi:ShowsSearchUrl";
		}

		public static class ErrorLog
		{
			public const string INVALID_PARAMS = "Invalid params received";
			public const string INTERNAL_ERROR = "Internal error: {0}";
			public const string TVMAZE_API_NOT_AVAILABLE = "TvMazeApi not available. Error message: {0}";
		}

		public static class Error
		{
			public const string SHOW_DOES_NOT_EXIST = "Show with id={0} does not exist.";
			public const string FAILED_TO_ADD_SHOW = "Internal error: failed to add a '{0}' show.";
			public const string FAILED_TO_DELETE_SHOW = "Internal error: failed to delete a show with id={0}.";
			public const string INVALID_SKIP_TAKE_PARAMS = "Invalid params: use 'skip' < 0 and 'take' <= 0.";
			public const string INVALID_SHOW_NAME_PARAM_EMPTY= "Invalid 'name' param value: should be not null or empty.";

			public const string TVMAZE_API_NOT_AVAILABLE = "Failed to obtain TvMazeSHow, TvMazeApi service not available.";
		}
	}
}