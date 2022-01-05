using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>>
        {
            public string UserName {get;set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(u => u.UserName).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Obtener nombre de usuario
                var user = await userManager.FindByNameAsync(request.UserName);
                if(user == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El nombre de usuario no existe"});
                }
                //Obtener los roles del usuario
                var roles = await userManager.GetRolesAsync(user);
                return new List<string>(roles);
            }
        }
    }
}