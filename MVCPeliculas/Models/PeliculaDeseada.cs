using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPeliculas.Models
{
    public class PeliculaDeseada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(Pelicula))]
        public int PeliculaId { get; set; }
        public Usuario Usuario { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
