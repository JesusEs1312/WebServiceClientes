using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Aplicacion.Contratos;
using Dominio;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Seguridad.Token
{
    public class JwtGenerador : IJwtGenerador
    {
        public string CrearToken(Usuario usuario, List<string> roles)
        {
            //Lista de Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };
            //Verificamos los roles
            if(roles != null)
            {
                foreach(var rol in roles)
                {
                    //Agregamos los roles del usuario al Token
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }
            //Credenciales de Acceso (Con la clave secreta)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //Descripcion del Token
            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credentials
            };
            //Crear el Token
            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);
            return tokenManejador.WriteToken(token);
        }
    }
}