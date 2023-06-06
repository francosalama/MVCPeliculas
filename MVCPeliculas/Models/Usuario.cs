using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVCPeliculas.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Contrasenia { get; set; }
        [EnumDataType(typeof(Rol))]
        public Rol Rol { get; set; }
        public ICollection<PeliculaVista> PeliculaVista { get; set; }

    }
}
