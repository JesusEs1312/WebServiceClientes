using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class RecuperaContraseña
    {
        public class Ejecuta : IRequest
        {
            public string EmailLogin {get;set;}
            public string EmailParaCodigo {get;set;}
        }

        public class Validaciones : AbstractValidator<Ejecuta>
        {   
            public Validaciones(){
                RuleFor(e => e.EmailLogin).NotEmpty();
                RuleFor(e => e.EmailParaCodigo).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> userManager; 
            public Manejador(UserManager<Usuario> userManager)
            {
                this.userManager = userManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Obtener Usuario de la base de datos mediante el Email
                var usuario = await this.userManager.FindByEmailAsync(request.EmailLogin);
                //Validamos al Usuario 
                if(usuario == null){
                    new ManejadorExcepcion(HttpStatusCode.Unauthorized); 
                }
                //Generar codigo de 4 digitos aleatorio
                Random myObject = new Random();
                int ranNum= myObject.Next(1000, 8500);
                //Servicio para enviar Codigo 
                SmtpClient smtp = new SmtpClient(); 
                smtp.Credentials = new System.Net.NetworkCredential("estrada.jesus1312@gmail.com", "Chuy13121341"); 
                smtp.Port = 587; 
                smtp.EnableSsl = true; 
                smtp.Host = "smtp.gmail.com"; 
                MailMessage mailMsg = new MailMessage(); 
                mailMsg.From = new MailAddress("estrada.jesus1312@gmail.com"); 
                mailMsg.To.Add(request.EmailParaCodigo); 
                mailMsg.IsBodyHtml = true; 
                mailMsg.Subject = "Codigo de Recuperacion de Contraseña"; 
                mailMsg.Body = ranNum.ToString(); 
                smtp.Send(mailMsg);
                //Actualizamos el codigo del usuario
                usuario.PhoneNumber = ranNum.ToString();
                var actualizar = await userManager.UpdateAsync(usuario);
                if(actualizar.Succeeded){
                    return Unit.Value;
                }
                throw new Exception("No se puedo actualizar el usuario"); 
            }
        }
    }
}