﻿using System.ComponentModel.DataAnnotations;

namespace TP_Labo4_Final.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }     

        [Display(Name = "Foto")]
        public string? NombreImagen { get; set; }

        //Clave foranea a Categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        //Relacion uno a muchos con articulos        
        public ICollection<ArticuloCantidad>? ArticulosCantidades { get; set; }
    }
}
