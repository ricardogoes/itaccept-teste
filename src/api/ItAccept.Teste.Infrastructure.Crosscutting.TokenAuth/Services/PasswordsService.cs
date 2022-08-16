using Fare;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Services
{
    public class PasswordsService : IPasswordsService
    {
        private readonly IPasswordsRepository _repository;

        public PasswordsService(IPasswordsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public HashPassword Encriptar(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password inválido");

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return new HashPassword { Password = password, EncryptedPassword = hashedPassword, Salt = salt };
        }

        public string GerarHashPassword(string password, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(password) || salt is null)
                throw new ArgumentException("Parâmetros inválidos");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public HashPassword GerarPasswordRandomico()
        {
            var xeger = new Xeger(@"^[a-zA-Z]{5,5}[!@#\$%\^&\*]{1,1}[0-9]{3,3}[!@#\$%\^&\*]{1,1}$");
            var password = xeger.Generate();

            return Encriptar(password);
        }

        public async Task<byte[]> ConsultarHashPasswordPorUsernameAsync(string username)
            => await _repository.ConsultarHashPasswordPorUsernameAsync(username);

        public async Task<PasswordInfo> ConsultarPasswordInfoPorUsernameAsync(string username)
            => await _repository.ConsultarPasswordInfoPorUsernameAsync(username);
    }
}
