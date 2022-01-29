using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class CambiarContraseña
    {
        public class Ejecuta : IRequest
        {
            public string CodigoLogin {get;set;}
            public string Email {get;set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(c => c.CodigoLogin).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly Context context;

            public Manejador(UserManager<Usuario> userManager, Context context)
            {
                this.userManager = userManager;
                this.context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByEmailAsync(request.Email);
                if(usuario == null)
                {
                    new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
                var codigoValido = await context.Users.Where(x => x.PhoneNumber == request.CodigoLogin).AnyAsync();
                if(codigoValido){
                    return Unit.Value;
                }
                throw new System.Exception("El codigo no es correcto");
            }
        }
    }
}