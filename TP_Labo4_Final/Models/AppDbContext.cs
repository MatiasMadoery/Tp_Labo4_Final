using Microsoft.EntityFrameworkCore;

namespace TP_Labo4_Final.Models
{
    public class AppDbContext:DbContext
    {
        //Utilizacion de la dependencia de conxion.
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }


        //Relaciones entre tablas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relacion uno a muchos entre Articulo y Categoria
            modelBuilder.Entity<Articulo>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Articulos)
                .HasForeignKey(a => a.CategoriaId);

            //Relacion uno a muchos entre Pedido y Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);         

            //Relacion uno a muchos entre Cliente y Viajante
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Viajante)
                .WithMany(v => v.Clientes)
                .HasForeignKey(c => c.ViajanteId);

        }

        //Creacion de tablas en la Bd
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }  
        public DbSet<Viajante> Viajantes { get; set; }
   
    }

    
}
