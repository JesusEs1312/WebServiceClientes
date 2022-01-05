using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

        //http://localhost:5000/api/Rol/eliminar
        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(EliminarRol.Ejecuta parametros) => await mediator.Send(parametros);
        
        //http://localhost:5000/api/Rol/lista
        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> Roles() => await mediator.Send(new ListaRoles.Ejecuta());
        
        //http://localhost:5000/api/Rol/agregarUsuario
        [HttpPost("agregarUsuario")]
        public async Task<ActionResult<Unit>> AgregarUsuario(AgregarRolUsuario.Ejecuta parametros) => await mediator.Send(parametros);
        
        //http://localhost:5000/api/Rol/eliminarUsuario
        [HttpPost("eliminarUsuario")]
        public async Task<ActionResult<Unit>> EliminarUsuario(EliminarRolUsuario.Ejecuta parametros) => await mediator.Send(parametros);

        //http://localhost:5000/api/Rol/username
        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> ObtenerRolesUsuario(string username) => await mediator.Send(new ObtenerRolesPorUsuario.Ejecuta{UserName = username});
        
    }
}