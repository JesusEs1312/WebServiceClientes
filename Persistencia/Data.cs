using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class Data
    {
        public static async Task InsertarData(Context context, UserManager<Usuario> usuarioManager)
        {
            //Validar no existe algun usuario
            if(!usuarioManager.Users.Any())
            {
                var usuario = new Usuario{NombreCompleto = "Jesus Estrada", UserName = "JEstrada", Email = "estrada.jesus@gmail.com"};
                //Crear un nuevo usuario con su contraseña
                await usuarioManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}