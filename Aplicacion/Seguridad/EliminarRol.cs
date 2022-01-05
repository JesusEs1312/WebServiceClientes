using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class EliminarRol
    {
        public class Ejecuta : IRequest
        {
            public string role {get;set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(r => r.role).NotEmpty();
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
                //Buscamos el Rol a eliminar
                var role = await roleManager.FindByNameAsync(request.role);
                if(role == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No existe el Role"});
                }
                //Eliminamos el Rol de la base de datos
                var resultado = await roleManager.DeleteAsync(role);
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new System.Exception("No se pudo eliminar el Role");
            }
        }
    }
}