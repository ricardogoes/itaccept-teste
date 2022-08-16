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
    [Route("api/v{version:apiVersion}/transportadoras")]
    public class TransportadorasController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmpresasService _empresasService;
        private readonly IEmbarcadorasTransportadorasService _embarcadorasTransportadorasService;
        private readonly IOfertasService _ofertasService;
        private readonly ILancesService _lancesService;
        private readonly IUsuariosService _usuariosService;

        public TransportadorasController(IMapper mapper, ILancesService lancesService, IOfertasService ofertasService,
            IEmpresasService empresasService, IEmbarcadorasTransportadorasService embarcadorasTransportadorasService,
            IUsuariosService usuariosService, ILogger<TransportadorasController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _empresasService = empresasService ?? throw new ArgumentNullException(nameof(empresasService));
            _embarcadorasTransportadorasService = embarcadorasTransportadorasService ?? throw new ArgumentNullException(nameof(embarcadorasTransportadorasService));
            _lancesService = lancesService ?? throw new ArgumentNullException(nameof(lancesService));
            _ofertasService = ofertasService ?? throw new ArgumentNullException(nameof(ofertasService));
            _usuariosService = usuariosService ?? throw new ArgumentNullException(nameof(usuariosService));
        }

        [HttpGet("{id}", Name = nameof(ConsultarTransportadoraPeloId))]
        public async Task<IActionResult> ConsultarTransportadoraPeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var empresaEncontrado = await _empresasService.ConsultarPeloIdAsync(id);
                if (empresaEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Transportadora não encontrada"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", empresaEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet(Name = nameof(ConsultarTransportadoras))]
        public async Task<IActionResult> ConsultarTransportadoras([FromQuery] string status)
        {
            try
            {
                var transportadoras = status switch
                {
                    "ativos" => await _empresasService.ConsultarTransportadorasAtivasAsync(),
                    "inativos" => (await _empresasService.ConsultarTransportadorasAsync()).Where(x => !x.Status),
                    _ => await _empresasService.ConsultarTransportadorasAsync()
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", transportadoras));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirTransportadora))]
        public async Task<IActionResult> InserirTransportadora([FromBody] EmpresaParaInserirVM transportadoraParaInserirVM)
        {
            try
            {
                if (transportadoraParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var empresa = _mapper.Map<Empresa>(transportadoraParaInserirVM);
                empresa.Status = true;

                var empresaIdInserido = await _empresasService.InserirAsync(empresa);

                return CreatedAtAction(
                    actionName: nameof(ConsultarTransportadoraPeloId),
                    routeValues: new { id = empresaIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Empresa criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}", Name = nameof(AtualizarTransportadora))]
        public async Task<IActionResult> AtualizarTransportadora(int id, [FromBody] EmpresaParaAtualizarVM transportadoraParaAtualizarVM)
        {
            try
            {
                if (transportadoraParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var empresaEncontrado = await _empresasService.ConsultarPeloIdAsync(id);
                if (empresaEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Empresa não encontrada"));

                var empresa = _mapper.Map<Empresa>(transportadoraParaAtualizarVM);
                var empresaIdInserido = await _empresasService.AtualizarAsync(empresa);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/status", Name = nameof(AtualizarStatusEmpresa))]
        public async Task<IActionResult> AtualizarStatusEmpresa(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var transportadoraEncontrada = await _empresasService.ConsultarPeloIdAsync(id);
                if (transportadoraEncontrada is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Empresa não encontrada"));

                transportadoraEncontrada.Status = !transportadoraEncontrada.Status;
                await _empresasService.InativarAsync(transportadoraEncontrada);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/lances", Name = nameof(ConsultarLancesPelaTransportadora))]
        public async Task<IActionResult> ConsultarLancesPelaTransportadora(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var transportadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (transportadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Transportadora não encontrada"));

                var lances = status switch
                {
                    "ativos" => await _lancesService.ConsultarAtivosPelaTransportadoraAsync(id),
                    "inativos" => (await _lancesService.ConsultarPelaTransportadoraAsync(id)).Where(x => !x.Status),
                    _ => await _lancesService.ConsultarPelaTransportadoraAsync(id)
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", lances));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/embarcadoras", Name = nameof(ConsultarEmbarcadorasPelaTransportadora))]
        public async Task<IActionResult> ConsultarEmbarcadorasPelaTransportadora(int id, [FromQuery] string tipo)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var transportadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (transportadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Transportadora não encontrada"));

                List<Empresa> embarcadoras = tipo switch
                {
                    "associados" => (await _embarcadorasTransportadorasService.ConsultarAssociadosPelaTransportadoraAsync(id)).ToList(),
                    "nao-associados" => (await _embarcadorasTransportadorasService.ConsultarNaoAssociadosPelaTransportadoraAsync(id)).ToList(),
                    _ => throw new ArgumentException("Inválido", nameof(tipo))
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", embarcadoras));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet("{id}/usuarios", Name = nameof(ConsultarUsuariosPelaTransportadora))]
        public async Task<IActionResult> ConsultarUsuariosPelaTransportadora(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var transportadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (transportadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Transportadora não encontrada"));

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

        [HttpGet("{id}/ofertas", Name = nameof(ConsultarOfertasPelaTransportadora))]
        public async Task<IActionResult> ConsultarOfertasPelaTransportadora(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var transportadora = await _empresasService.ConsultarPeloIdAsync(id);
                if (transportadora is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Transportadora não encontrada"));

                var ofertas = await _ofertasService.ConsultarAtivosPelaTransportadoraAsync(id);

                return Ok(new ApiResponse(ApiResponseState.Success, "", ofertas));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
