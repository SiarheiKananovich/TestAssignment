using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Cast> Casts { get; set; }
		public DbSet<Show> Shows { get; set; }

		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}