using System.Security.Claims;
using System.Linq;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad.Token
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string ObtenerUsuario() => this.httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}