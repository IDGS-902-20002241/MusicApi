using CineAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Genero> Generos { get; set; }
		public DbSet<Album> Albumes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Album>()
				.Property(a => a.Precio)
				.HasPrecision(10, 2);
		}
	}
}