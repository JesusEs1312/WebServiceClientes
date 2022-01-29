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
    public class NuevaContraseña
    {
        public class Ejecuta : IRequest
        {
            public string Password1 {get;set;}
            public string Password2 {get;set;}
            public string Email {get; set;}
        }
        
        public class Validaciones : AbstractValidator<Ejecuta>
        {
            public Validaciones()
            {
                RuleFor(x => x.Password1).NotEmpty();
                RuleFor(x => x.Password2).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IPasswordHasher<Usuario> passwordHasher;

            public Manejador(UserManager<Usuario> userManager, IPasswordHasher<Usuario> passwordHasher)
            {
                this.userManager = userManager;
                this.passwordHasher = passwordHasher;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Encontrar nombre de usuario
                var user = await userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "El usuario no existe"});
                }
                //Verificamos si el nuevo Email ya existe en otro Usuario
                if(request.Password1 != request.Password2){
                    throw new System.Exception("Las contraseñas no coinciden");
                }
                //Actualizamos password
                user.PasswordHash   = passwordHasher.HashPassword(user, request.Password1);
                //Actualizamos el usuario en la base de datos
                var actualizar = await userManager.UpdateAsync(user);
                //Obtenemos los roles del usuario
                var resultadoRoles = await userManager.GetRolesAsync(user);
                if(actualizar.Succeeded)
                {
                    return Unit.Value; 
                }
                //Por si todo sale mal
                throw new System.Exception("No se puedo actualizar el usuario");
            }
        }
    }
}