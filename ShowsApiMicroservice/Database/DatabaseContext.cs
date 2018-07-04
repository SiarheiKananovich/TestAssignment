using Database.Interface.Models;
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Show model builder
			modelBuilder.Entity<Show>()
				.HasKey(item => item.Id);

			modelBuilder.Entity<Show>()
				.Property(item => item.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Show>()
				.Property(item => item.Name)
				.IsRequired();

			// Cast model builder
			modelBuilder.Entity<Cast>()
				.HasKey(item => item.Id);

			modelBuilder.Entity<Cast>()
				.Property(item => item.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Cast>()
				.HasOne(cast => cast.Show)
				.WithMany(show => show.Casts)
				.HasForeignKey(cast => cast.ShowId);
		}
	}
}