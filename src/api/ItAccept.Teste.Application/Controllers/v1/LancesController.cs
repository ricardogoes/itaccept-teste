using AutoMapper;
using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Domain.ViewModels.Lances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers.v1
{
    [ValidateModel]
    [Authorize("Bearer")]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/lances")]
    public class LancesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILancesService _lancesService;

        public LancesController(IMapper mapper, 
            ILancesService lancesService, ILogger<LancesController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _lancesService = lancesService ?? throw new ArgumentNullException(nameof(lancesService));
        }

        [HttpGet("{id}", Name = nameof(ConsultarLancePeloId))]
        public async Task<IActionResult> ConsultarLancePeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var lanceEncontrado = await _lancesService.ConsultarPeloIdAsync(id);
                if (lanceEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Lance não encontrado"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", lanceEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirLance))]
        public async Task<IActionResult> InserirLance([FromBody] LanceParaInserirVM lanceParaInserirVM)
        {
            try
            {
                if (lanceParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var lance = _mapper.Map<Lance>(lanceParaInserirVM);
                lance.Status = true;
                lance.LanceVencedor = false;

                var lanceIdInserido = await _lancesService.InserirAsync(lance);

                return CreatedAtAction(
                    actionName: nameof(ConsultarLancePeloId),
                    routeValues: new { id = lanceIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Lance criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}",Name = nameof(AtualizarLance))]
        public async Task<IActionResult> AtualizarLance(int id, [FromBody] LanceParaAtualizarVM lanceParaAtualizarVM)
        {
            try
            {
                if (lanceParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var lanceEncontrado = await _lancesService.ConsultarPeloIdAsync(id);
                if (lanceEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Lance não encontrado"));

                var lance = _mapper.Map<Lance>(lanceParaAtualizarVM);
                lance.Status = true;

                var lanceIdInserido = await _lancesService.AtualizarAsync(lance);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/status", Name = nameof(InativarLance))]
        public async Task<IActionResult> InativarLance(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var lance = await _lancesService.ConsultarPeloIdAsync(id);
                if (lance is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Usuario não encontrado"));

                lance.Status = !lance.Status;
                await _lancesService.InativarAsync(_mapper.Map<Lance>(lance));

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/vencedor", Name = nameof(LanceVencedor))]
        public async Task<IActionResult> LanceVencedor(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var lance = await _lancesService.ConsultarPeloIdAsync(id);
                if (lance is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Usuario não encontrado"));

                lance.LanceVencedor = true;
                await _lancesService.AtualizarAsync(_mapper.Map<Lance>(lance));

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
