using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models
{
    public class TokenAuthOptions
    {
        public static string Audience { get; } = "ExampleAudience";
        public static string Issuer { get; } = "ExampleIssuer";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(240);
        public static string TokenType { get; } = "Bearer";

        private static RSAParameters GenerateKey()
        {
            using var key = new RSACryptoServiceProvider(2048);
            return key.ExportParameters(true);
        }
    }
}
