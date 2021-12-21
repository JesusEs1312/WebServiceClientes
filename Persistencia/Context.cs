
using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class Context : IdentityDbContext<Usuario>
    {
        public Context(DbContextOptions options) : base(options)
        {    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Crear las tablas de migracion a SQL Server
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Producto> Producto {get; set;}//Entidad para la tabla productos de SQL Server
    }   
}

//NOTA: Comando para instalar dotnet-ef
//dotnet tool install --global dotnet-ef --version 5.0.0
//NOTA: Comando para crear la carpeta con las tablas de migracion
//dotnet ef migrations add IdentityCoreInicial -p Persistencia/ -s WebAPI