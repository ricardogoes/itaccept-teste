using ItAccept.Teste.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        private readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IActionResult HandleError(Exception ex)
        {
            //TODO: Configure NLog properly                        
            _logger.LogError(ex, ex.Message);
            
            return StatusCode(500, new ApiResponse(ApiResponseState.Failed, $"Um erro ocorreu, por favor tente novamente. Se o erro persistir, entre em contato com o administrador. DETALHE ERRO: {ex.Message}"));
        }
    }
}
