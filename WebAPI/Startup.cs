using System.Net;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Seguridad.Token;
using WebAPI.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

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
            //Configuraciones de los Controladores
            services.AddControllers( opt => {
                //Configuracion para que todos los controladores tengan habilitada la Authorization
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
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
            //Generar la clave secreta que realizamos en el proyecto de Seguridad
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            //Servicio para implementar la Autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => 
            {
                //Validaciones en los paramatros
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey   = true,//Validar el Request del cliente
                    IssuerSigningKey           = key,//Palabra clave que se paso en el Token
                    ValidateAudience           = false,//Quien puede crear los Token(TODOS)
                    ValidateIssuer             = false//Envio de validacion de Token
                };
            });
            //Agregar el servicio para obtener el Usuario Actual
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
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

            app.UseAuthentication();//Se usara la utenticacion de Token

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
