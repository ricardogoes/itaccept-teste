using ItAccept.Teste.Domain.ViewModels.Usuarios;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth
{
    public class TokenAuthService : ITokenAuthService
    {
        public string GenerateToken(UsuarioParaConsultarVM usuario)
        {
            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(
                new GenericIdentity(usuario.Username, "TokenAuth"),
                new[] {
                    new Claim("UsuarioId", usuario.UsuarioId.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOptions.Issuer,
                Audience = TokenAuthOptions.Audience,
                SigningCredentials = TokenAuthOptions.SigningCredentials,
                Subject = identity
            });
            return handler.WriteToken(securityToken);
        }
    }
}
