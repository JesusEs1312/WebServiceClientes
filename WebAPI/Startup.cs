using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Persistencia;
using Seguridad.Token;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Inyectar el context de la data a la API
            services.AddDbContext<Context>(opt => 
            {
                //Indicar el tipo de conexion
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));    
            });
            //Mediator para conectarse con la WEBAPI
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
            //-----------CONFIGURACION DE IDENTITY CORE----------
            //Crear la variable que representa la instancia de la clase usuario
            var builder = services.AddIdentityCore<Usuario>();
            //Objeto de tipo IdentityBuilder
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            //Agregar la instancia del EntityFramework
            identityBuilder.AddEntityFrameworkStores<Context>();
            //Acceso a los usuarios y manejo de Login
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            //Metodo que se utilizo para el IdentityCore del usuario
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //------------Inyeccion del servicio de JSON WEB TOKEN-------------
            services.AddScoped<IJwtGenerador, JwtGenerador>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();
            if (env.IsDevelopment())
            {
                //Se comento la linea de abajo ya que configuaste tu propio Middleware
                //app.UseDeveloperExceptionPage();//Ambiente de desarrollo
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
