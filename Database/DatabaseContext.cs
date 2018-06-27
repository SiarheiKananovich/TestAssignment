using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Cast> Casts { get; set; }
		public DbSet<Show> Shows { get; set; }
		public DbSet<ExternalShowMap> ExternalShowMap { get; set; }

		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Show>()
				.HasOne(show => show.TvMazeShowMap)
				.WithOne(tvMazeShowMap => tvMazeShowMap.Show)
				.HasForeignKey<ExternalShowMap>(tvMazeShowMap => tvMazeShowMap.ShowId);
		}
	}
}