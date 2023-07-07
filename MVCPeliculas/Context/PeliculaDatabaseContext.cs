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
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<PeliculaVista> PeliculaVista { get; set; }
        public DbSet<PeliculaDeseada> PeliculaDeseada { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculaVista>().HasKey(PV => new { PV.UsuarioId, PV.PeliculaId });
            modelBuilder.Entity<PeliculaDeseada>().HasKey(PD => new { PD.UsuarioId, PD.PeliculaId });
        }

    }
}
