using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Models;

namespace MVCPeliculas.Context
{
	public class PeliculaDatabaseContext : DbContext
	{
		public PeliculaDatabaseContext(DbContextOptions<PeliculaDatabaseContext> options) : base(options)
		{
		}
		public DbSet<Pelicula> Pelicula { get; set; }
	}
}
