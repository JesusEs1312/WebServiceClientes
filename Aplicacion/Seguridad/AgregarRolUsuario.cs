using System;
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
    public class AgregarRolUsuario
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
                RuleFor(n => n.NombreRol).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<Usuario> userManager;

            public Manejador(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
            {
                this.roleManager = roleManager;
                this.userManager = userManager;
            } 
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Obtener Rol
                var role = await roleManager.FindByNameAsync(request.NombreRol);
                if(role == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El rol no existe"});
                }
                //Obtener Nombre de usuario
                var user = await userManager.FindByNameAsync(request.UserName);
                if(user == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El usuario no existe"});
                }
                //Agregar Rol al usuario
                var resultado = await userManager.AddToRoleAsync(user, request.NombreRol);
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo asignar el rol al usuario");
            }
        }
    }
}