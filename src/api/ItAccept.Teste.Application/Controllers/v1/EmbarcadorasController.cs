using AutoMapper;
using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Domain.ViewModels.Empresas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers.v1
{
    [ValidateModel]
    [Authorize("Bearer")]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/embarcadoras")]
    public class EmbarcadorasController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmpresasService _empresasService;
        private readonly IEmbarcadorasTransportadorasService _embarcadorasTransportadorasService;
        private readonly IOfertasService _ofertasService;
        private readonly IProdutosService _produtosService;
        private readonly IUsuariosService _usuariosService;

        public EmbarcadorasController(IMapper mapper, IOfertasService ofertasService, IProdutosService produtosService,
            IEmpresasService empresasService, IEmbarcadorasTransportadorasService ebarcadorasTransportadorasService,
            IUsuariosService usuariosService, ILogger<EmbarcadorasController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _empresasService = empresasService ?? throw new ArgumentNullException(nameof(empresasService));
            _embarcadorasTransportadorasService = ebarcadorasTransportadorasService ?? throw new ArgumentNullException(nameof(ebarcadorasTransportadorasService));
            _ofertasService = ofertasService ?? throw new ArgumentNullException(nameof(ofertasService));
            _produtosService = produtosService ?? throw new ArgumentNullException(nameof(produtosService));
            _usuariosService = usuariosService ?? throw new ArgumentNullException(nameof(usuariosService));
        }

        [HttpGet("{id}", Name = nameof(ConsultarEmbarcadoraPeloId))]
        public async Task<IActionResult> ConsultarEmbarcadoraPeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var EmbarcadoraEncontrado = await _empresasService.ConsultarPeloIdAsync(id);
                if (EmbarcadoraEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", EmbarcadoraEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet(Name = nameof(ConsultarEmbarcadoras))]
        public async Task<IActionResult> ConsultarEmbarcadoras([FromQuery] string status)
        {
            try
            {
                var embarcadoras = status switch
                {
                    "ativos" => await _empresasService.ConsultarEmbarcadorasAtivasAsync(),
                    "inativos" => (await _empresasService.ConsultarEmbarcadorasAsync()).Where(x => !x.Status),
                    _ => await _empresasService.ConsultarEmbarcadorasAsync()
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", embarcadoras));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirEmbarcadora))]
        public async Task<IActionResult> InserirEmbarcadora([FromBody] EmpresaParaInserirVM embarcadoraParaInserirVM)
        {
            try
            {
                if (embarcadoraParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadora = _mapper.Map<Empresa>(embarcadoraParaInserirVM);
                embarcadora.Status = true;

                var EmbarcadoraIdInserido = await _empresasService.InserirAsync(embarcadora);

                return CreatedAtAction(
                    actionName: nameof(ConsultarEmbarcadoraPeloId),
                    routeValues: new { id = EmbarcadoraIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Embarcadora criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}", Name = nameof(AtualizarEmbarcadora))]
        public async Task<IActionResult> AtualizarEmbarcadora(int id, [FromBody] EmpresaParaAtualizarVM empresaParaAtualizarVM)
        {
            try
            {
                if (empresaParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadoraEncontrado = await _empresasService.ConsultarPeloIdAsync(id);
                if (embarcadoraEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                var Embarcadora = _mapper.Map<Empresa>(empresaParaAtualizarVM);
                var EmbarcadoraIdInserido = await _empresasService.AtualizarAsync(Embarcadora);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/status", Name = nameof(AtualizarStatusEmbarcadora))]
        public async Task<IActionResult> AtualizarStatusEmbarcadora(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadoraEncontrado = await _empresasService.ConsultarPeloIdAsync(id);
                if (embarcadoraEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                embarcadoraEncontrado.Status = !embarcadoraEncontrado.Status;
                await _empresasService.InativarAsync(embarcadoraEncontrado);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/ofertas", Name = nameof(ConsultarOfertasPelaEmbarcadora))]
        public async Task<IActionResult> ConsultarOfertasPelaEmbarcadora(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (embarcadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                var ofertas = status switch
                {
                    "ativos" => await _ofertasService.ConsultarAtivosPelaEmbarcadoraAsync(id),
                    "inativos" => (await _ofertasService.ConsultarPelaEmbarcadoraAsync(id)).Where(x => !x.Status),
                    _ => await _ofertasService.ConsultarPelaEmbarcadoraAsync(id)
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", ofertas));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/transportadoras", Name = nameof(ConsultarTransportadorasPelaEmbarcadora))]
        public async Task<IActionResult> ConsultarTransportadorasPelaEmbarcadora(int id, [FromQuery] string tipo)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (embarcadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                List<Empresa> transportadoras = tipo switch
                {
                    "associados" => (await _embarcadorasTransportadorasService.ConsultarAssociadosPelaEmbarcadoraAsync(id)).ToList(),
                    "nao-associados" => (await _embarcadorasTransportadorasService.ConsultarNaoAssociadosPelaEmbarcadoraAsync(id)).ToList(),
                    _ => throw new ArgumentException("Inválido", nameof(tipo))
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", transportadoras));
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/produtos", Name = nameof(ConsultarProdutosPelaEmbarcadora))]
        public async Task<IActionResult> ConsultarProdutosPelaEmbarcadora(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (embarcadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                var produtos = status switch
                {
                    "ativos" => await _produtosService.ConsultarAtivosPelaEmbarcadoraAsync(id),
                    "inativos" => (await _produtosService.ConsultarPelaEmbarcadoraAsync(id)).Where(x => !x.Status),
                    _ => await _produtosService.ConsultarPelaEmbarcadoraAsync(id)
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", produtos));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/usuarios", Name = nameof(ConsultarUsuariosPelaEmbarcadora))]
        public async Task<IActionResult> ConsultarUsuariosPelaEmbarcadora(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (embarcadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                var usuarios = status switch
                {
                    "ativos" => await _usuariosService.ConsultarAtivosPelaEmpresaAsync(id),
                    "inativos" => (await _usuariosService.ConsultarPelaEmpresaAsync(id)).Where(x => !x.Status),
                    _ => await _usuariosService.ConsultarPelaEmpresaAsync(id)
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", usuarios));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
