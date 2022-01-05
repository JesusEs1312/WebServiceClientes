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
    public class EliminarRolUsuario
    {
        public class Ejecuta : IRequest
        {
            public string UserName {get;set;}
            public string NombreRol {get;set;}

        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(u => u.UserName).NotEmpty();
                RuleFor(u => u.NombreRol).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            public Manejador(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Obtenemos el Role
                var role = await roleManager.FindByNameAsync(request.NombreRol);
                if(role == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El role no existe"});
                }
                //Obtener el nombre del usuario
                var user = await userManager.FindByNameAsync(request.UserName);
                if(user == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El nombre de usuario no existe"});
                }
                //Eliminar Role al usuario
                var resultado = await userManager.RemoveFromRoleAsync(user, request.NombreRol);
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new System.Exception("No se pudo eliminar Role al usuario");
            }
        }
    }
}