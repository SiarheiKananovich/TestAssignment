using Database.Interfaces;

namespace Database
{
	public class Database : IDatabase
	{
		public IShowRepository ShowRepository { get; private set; }

		public Database(IShowRepository showRepository)
		{
			ShowRepository = showRepository;
		}
	}
}