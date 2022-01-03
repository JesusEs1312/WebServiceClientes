using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
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
    }
}