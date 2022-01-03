using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Nombre {get;set;}
            public string Apellidos {get;set;}
            public string Email {get;set;}
            public string Password {get;set;}
            public string UserName {get;set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(n => n.Nombre).NotEmpty();
                RuleFor(a => a.Apellidos).NotEmpty();
                RuleFor(e => e.Email).NotEmpty();
                RuleFor(p => p.Password).NotEmpty();
                RuleFor(u => u.UserName).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            Context context;
            UserManager<Usuario> userManager;
            IJwtGenerador jwtGenerador;
            public Manejador(Context context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador; 
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Verificamos si el Email del nuevo usuario ya existe
                var existe = await context.Users.Where(e => e.Email == request.Email).AnyAsync();
                if(existe)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Existe un usuario con este Email"});
                }
                var existeUserName = await context.Users.Where(u => u.UserName == request.UserName).AnyAsync();
                if(existeUserName)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Existe un usuario con este UserName"});
                }
                //Creamos al nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.UserName
                };
                //Registrarmos al nuevo usuario en la base de datos
                var result = await userManager.CreateAsync(nuevoUsuario, request.Password);
                if(result.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = nuevoUsuario.NombreCompleto,
                        Token = jwtGenerador.CrearToken(nuevoUsuario),
                        UserName = nuevoUsuario.UserName,
                        Email = nuevoUsuario.Email
                    };
                }
                throw new System.Exception("No se puedo insertar el usuario en la base de datos");
            }
        }
    }
}