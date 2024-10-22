    namespace TP_Labo4_Final.Models
    {
        public class Usuario
        {
            public int Id { get; set; }
            public string? NombreUsuario { get; set; }
            public string? Contrasena { get; set; }
            public string? Rol { get; set; } // Puede ser "Administrador" o "Usuario"
        }
    }


