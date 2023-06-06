namespace MVCPeliculas.Models
{
    public class PeliculaVista
    {
        public int UsuarioId { get; set; }
        public int PeliculaId { get; set; }
        public Usuario Usuario { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
