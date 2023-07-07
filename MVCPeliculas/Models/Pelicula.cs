﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVCPeliculas.Models
{
	public class Pelicula
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        public string Nombre { get; set; }
        [Display(Name = "Imagen")]
        public string LinkFoto { get; set; }
        public int Valoracion { get; set; }
        [Display(Name = "Año")]
        public int Anio { get; set; }
        public string Sinopsis { get; set; }
		[EnumDataType(typeof(Genero))]
		public Genero Genero { get; set; }
		[EnumDataType(typeof(Plataforma))]
		public Plataforma Plataforma { get; set; }
        public List<PeliculaVista> UsuariosVistos { get; set; }
        public List<PeliculaDeseada> UsuariosDeseados { get; set; }

    }
}
