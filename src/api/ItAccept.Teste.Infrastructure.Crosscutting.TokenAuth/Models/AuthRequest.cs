using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Username obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password obrigatório")]
        public string Password { get; set; }
    }
}
