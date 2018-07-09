namespace TvMazeScraper.BusinessLogic.Interface.Configs
{
	public class CommunicationConfig
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public string VirtualHost { get; set; }

		public string HostName { get; set; }

		public int MaxBatchSize { get; set; }

		public string ImportRequestsQueue { get; set; }

		public string ImportRequestsExchange { get; set; }

		public string ImportResultsQueue { get; set; }

		public string ImportResultsExchange { get; set; }
	}
}