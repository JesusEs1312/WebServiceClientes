
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
        {    
        }

        public DbSet<Producto> Producto {get; set;}//Entidad para la tabla productos de SQL Server
    }   
}