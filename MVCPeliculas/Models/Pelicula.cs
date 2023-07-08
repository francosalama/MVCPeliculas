using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVCPeliculas.Models
{
	public class Pelicula
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        [Required(ErrorMessage = "Falta llenar campo de nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "Falta llenar campo de imagen")]
        public string LinkFoto { get; set; }

        [Range(1, 5, ErrorMessage = "Solo se admiten una valoracion entre {1} y {2}")]
        [Required(ErrorMessage = "Falta llenar campo de valoracion")]
        public int Valoracion { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "Falta llenar campo de año")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "Falta llenar campo de sinopsis")]
        public string Sinopsis { get; set; }

        [Required(ErrorMessage = "Falta llenar campo de genero")]
        [EnumDataType(typeof(Genero))]
		public Genero Genero { get; set; }

        [Required(ErrorMessage = "Falta llenar campo de plataforma")]
        [EnumDataType(typeof(Plataforma))]
		public Plataforma Plataforma { get; set; }
        public List<PeliculaVista> UsuariosVistos { get; set; }
        public List<PeliculaDeseada> UsuariosDeseados { get; set; }

    }
}
