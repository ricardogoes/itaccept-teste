using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Application.Controllers;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Service.Controllers.v1
{
    [ValidateModel]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class TokenAuthController : BaseController
    {
        private readonly IUsuariosService _usuariosService;
        private readonly ITokenAuthService _tokenAuthService;
        private readonly IPasswordsService _passwordsService;
        
        public TokenAuthController(IUsuariosService usuariosService, ITokenAuthService tokenAuthService,
            IPasswordsService passwordsService, ILogger<TokenAuthController> logger)
            : base(logger)
        {
            _passwordsService = passwordsService ?? throw new ArgumentNullException(nameof(passwordsService));
            _tokenAuthService = tokenAuthService ?? throw new ArgumentNullException(nameof(tokenAuthService));
            _usuariosService = usuariosService ?? throw new ArgumentNullException(nameof(usuariosService));
        }

        [HttpPost]
        public async Task<IActionResult> GetAuthToken([FromBody] AuthRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var searchedUser = await _usuariosService.ConsultarPeloUsernameAsync(request.Username);
                if (searchedUser is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Username ou Password são inválidos"));

                var hashPassword = await _passwordsService.ConsultarHashPasswordPorUsernameAsync(searchedUser.Username);
                var password = _passwordsService.GerarHashPassword(request.Password, hashPassword);

                var usuarioEncontrado = await _usuariosService.ConsultarPeloUsernameEPasswordAsync(request.Username, password);
                if (usuarioEncontrado is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Username ou Password são inválidos"));

                var requestAt = DateTime.UtcNow;
                var token = _tokenAuthService.GenerateToken(usuarioEncontrado);
                                
                return Ok(new ApiResponse(ApiResponseState.Success, "",
                    new AuthResponse
                    {
                        RequestAt = requestAt,
                        ExpiresIn = TokenAuthOptions.ExpiresSpan.TotalSeconds,
                        TokenType = TokenAuthOptions.TokenType,
                        AccessToken = token,
                        UsuarioLogado = usuarioEncontrado
                    }));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
