using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        //http://localhost:5000/api/usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros) => await mediator.Send(parametros);

        //http://localhost:5000/api/usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> registrar(Registrar.Ejecuta parametros) => await mediator.Send(parametros);

        //http://localhost:5000/api/usuario
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> ObtenerUsuario() => await mediator.Send(new UsuarioActual.Ejecuta());

        //http://localhost:5000/api/usuario
        [HttpPut]
        public async Task<ActionResult<UsuarioData>> ActualizarUsuario(ActualizarUsuario.Ejecuta parametros) => await mediator.Send(parametros);

        //http://localhost:5000/api/usuario/actualizarContraseña
        [HttpPut("actualizarContrasena")]
        public async Task<ActionResult<Unit>> ActualizarContra(RecuperaContraseña.Ejecuta parametros) => await mediator.Send(parametros);

        //http://localhost:5000/api/usuario/validarCodigo
        [HttpPost("validarCodigo")]
        public async Task<ActionResult<Unit>> ValidarCodigo(CambiarContraseña.Ejecuta parametros) => await mediator.Send(parametros);

        //http://localhost:5000/api/usuario/nuevaContra
        [HttpPost("nuevaContra")]
        public async Task<ActionResult<Unit>> nuevaContra(NuevaContraseña.Ejecuta parametros) => await mediator.Send(parametros);
    }
}