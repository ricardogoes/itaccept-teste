namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models
{
    public class HashPassword
    {
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}
