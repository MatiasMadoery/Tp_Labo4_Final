using System.ComponentModel.DataAnnotations;

namespace TP_Labo4_Final.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public DateTime Fecha { get; set; }


        //Clave foranea a Cliente
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }      

        //Relacion uno a muchos con articulos        
        public ICollection<Articulo>? Articulos { get; set; }    
        

    }
}
