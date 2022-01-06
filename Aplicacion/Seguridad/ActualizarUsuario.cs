using System.Collections.Generic;
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
    public class ActualizarUsuario
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto {get;set;}
            public string Email {get;set;}
            public string Password {get;set;}
            public string Username {get;set;}
        }
        
        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly Context context;
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGeneador;
            private readonly IPasswordHasher<Usuario> passwordHasher;

            public Manejador(Context context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGeneador = jwtGenerador;
                this.passwordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Encontrar nombre de usuario
                var user = await userManager.FindByNameAsync(request.Username);
                if(user == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El usuario no existe"});
                }
                //Verificamos si el nuevo Email ya existe en otro Usuario
                var existeEmail = await context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if(existeEmail)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new {mensaje = "Este Email ya le pertenece a otro usuario"});
                }
                //Actualizamos los datos
                user.NombreCompleto = request.NombreCompleto;
                user.Email          = request.Email;
                user.PasswordHash   = passwordHasher.HashPassword(user, request.Password);
                user.UserName       = request.Username;
                //Actualizamos el usuario en la base de datos
                var actualizar = await userManager.UpdateAsync(user);
                //Obtenemos los roles del usuario
                var resultadoRoles = await userManager.GetRolesAsync(user);
                var roles = new List<string>(resultadoRoles);
                if(actualizar.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = user.NombreCompleto,
                        Email          = user.Email,
                        UserName       = user.UserName,
                        Token          = jwtGeneador.CrearToken(user, roles)
                    };
                }
                //Por si todo sale mal
                throw new System.Exception("No se puedo actualizar el usuario");
            }
        }
    }
}