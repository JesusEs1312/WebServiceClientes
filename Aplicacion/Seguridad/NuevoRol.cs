using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class NuevoRol
    {
        public class Ejecuta: IRequest
        {
            public string Nombre {get;set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(n => n.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                this.roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Buscar Rol
                var role = await roleManager.FindByNameAsync(request.Nombre);
                if(role != null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El rol ya existe"});
                }
                //Creamos el nuevo Rol
                var resultado = await roleManager.CreateAsync(new IdentityRole(request.Nombre));
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new System.Exception("No se puedo guardar el Rol");
            }
        }
    }
}