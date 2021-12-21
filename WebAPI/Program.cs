using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
                    var context = services.GetRequiredService<Context>();
                    //Realizamos la Migracion s SQL Server
                    context.Database.Migrate();
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
