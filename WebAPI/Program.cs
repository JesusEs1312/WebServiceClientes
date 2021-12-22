using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Procedimiento para ejecutar la migracion de tablas a SQL Server
            var hostServer = CreateHostBuilder(args).Build();
            //Creamos un ambiente (Scope)
            using(var ambiente = hostServer.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;
                try
                {
                    //Instanciamos el UserManager
                    var userManager = services.GetRequiredService<UserManager<Usuario>>();
                    //Instanciamos el Contexto
                    var context = services.GetRequiredService<Context>();
                    //Realizamos la Migracion s SQL Server
                    context.Database.Migrate();
                    //Hacemos la instancia de la clase Data para agregar el usuario
                    Data.InsertarData(context, userManager).Wait(); 
                } catch (Exception e)
                {
                    var loggin = services.GetRequiredService<ILogger<Program>>();
                    loggin.LogError(e, "Ha ocurrido un error en la migracion");
                }
            }
            //Ejecutamos el server
            hostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

//NOTA: Comando para ejecutar el metodo Main de esta clase
//Entramos a la carpeta de WebAPI y ejecutamos dotnet watch run en la consola