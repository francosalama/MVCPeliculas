using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace MVCPeliculas.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Falta llenar campo de nombre"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$",
            ErrorMessage = "el campo {0} no admite numeros")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Falta llenar campo de apellido"), MaxLength(40)]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$",
            ErrorMessage = "el campo {0} no admite numeros")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe completar su mail"), MaxLength(100)]
        [EmailAddress(ErrorMessage = "La direccion de mail no existe")]
        public string Mail { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }
        [EnumDataType(typeof(Rol))]
        public Rol Rol { get; set; } = Rol.Usuario;
        public List<PeliculaVista> PeliculaVista { get; set; }
        public List<PeliculaDeseada> PeliculaDeseada { get; set; }

    }
}
