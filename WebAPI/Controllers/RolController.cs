using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : MiControllerBase
    {
        //http://localhost:5000/api/Rol/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(NuevoRol.Ejecuta parametros) => await mediator.Send(parametros);
    }
}