using AutoMapper;
using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Domain.ViewModels.Ofertas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers.v1
{
    [ValidateModel]
    [Authorize("Bearer")]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ofertas")]
    public class OfertasController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IOfertasService _ofertasService;
        private readonly ILancesService _lancesService;

        public OfertasController(IMapper mapper, ILancesService lancesService,
            IOfertasService ofertasService, ILogger<OfertasController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ofertasService = ofertasService ?? throw new ArgumentNullException(nameof(ofertasService));
            _lancesService = lancesService ?? throw new ArgumentNullException(nameof(lancesService));
        }

        [HttpGet("{id}", Name = nameof(ConsultarOfertaPeloId))]
        public async Task<IActionResult> ConsultarOfertaPeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var ofertasEncontrado = await _ofertasService.ConsultarPeloIdAsync(id);
                if (ofertasEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Oferta não encontrado"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", ofertasEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirOferta))]
        public async Task<IActionResult> InserirOferta([FromBody] OfertaParaInserirVM ofertasParaInserirVM)
        {
            try
            {
                if (ofertasParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var ofertas = _mapper.Map<Oferta>(ofertasParaInserirVM);
                ofertas.Status = true;

                var ofertasIdInserido = await _ofertasService.InserirAsync(ofertas);

                return CreatedAtAction(
                    actionName: nameof(ConsultarOfertaPeloId),
                    routeValues: new { id = ofertasIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Oferta criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}",Name = nameof(AtualizarOferta))]
        public async Task<IActionResult> AtualizarOferta(int id, [FromBody] OfertaParaAtualizarVM ofertasParaAtualizarVM)
        {
            try
            {
                if (ofertasParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var ofertasEncontrado = await _ofertasService.ConsultarPeloIdAsync(id);
                if (ofertasEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Oferta não encontrado"));

                var ofertas = _mapper.Map<Oferta>(ofertasParaAtualizarVM);
                ofertas.Status = true;

                var ofertasIdInserido = await _ofertasService.AtualizarAsync(ofertas);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/status", Name = nameof(InativarOferta))]
        public async Task<IActionResult> InativarOferta(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var oferta = await _ofertasService.ConsultarPeloIdAsync(id);
                if (oferta is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Usuario não encontrado"));

                oferta.Status = !oferta.Status;
                await _ofertasService.InativarAsync(_mapper.Map<Oferta>(oferta));

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/lances", Name = nameof(ConsultarLancesPelaOferta))]
        public async Task<IActionResult> ConsultarLancesPelaOferta(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var ofertaEncontrada = await _ofertasService.ConsultarPeloIdAsync(id);
                if (ofertaEncontrada is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Oferta não encontrada"));

                var lances = status switch
                {
                    "ativos" => await _lancesService.ConsultarAtivosPelaOfertaAsync(id),
                    "inativos" => (await _lancesService.ConsultarPelaOfertaAsync(id)).Where(x => !x.Status),
                    _ => await _lancesService.ConsultarPelaOfertaAsync(id)
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", lances));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
