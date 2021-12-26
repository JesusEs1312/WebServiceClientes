using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email {get; set;}
            public string Password {get; set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(e => e.Email).NotEmpty();
                RuleFor(p => p.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly SignInManager<Usuario> signInManager;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
            {
                this.userManager   = userManager;
                this.signInManager = signInManager;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Obtener el Usuario de la base de datos atravez del Email
                var usuario = await this.userManager.FindByEmailAsync(request.Email);
                //Validamos si el usuario es nulo
                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
                //Verificamos el Login del usuario
                var login = await signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if(login.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = "Falta la logica",
                        Email = usuario.Email,
                        UserName = usuario.UserName
                    };
                }
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}