using AutoMapper;
using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers.v1
{
    [ValidateModel]
    [Authorize("Bearer")]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/embarcadoras-transportadoras")]
    public class EmbarcadorasTransportadorasController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmbarcadorasTransportadorasService _service;

        public EmbarcadorasTransportadorasController(IMapper mapper,
            IEmbarcadorasTransportadorasService service, ILogger<EmbarcadorasTransportadorasController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{id}", Name = nameof(ConsultarEmbarcadoraTransportadoraPeloId))]
        public async Task<IActionResult> ConsultarEmbarcadoraTransportadoraPeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var EmbarcadoraEncontrado = await _service.ConsultarPeloIdAsync(id);
                if (EmbarcadoraEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Registro não encontrado"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", EmbarcadoraEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirEmbarcadoraTransportadora))]
        public async Task<IActionResult> InserirEmbarcadoraTransportadora([FromBody] EmbarcadoraTransportadoraParaInserirVM embarcadoraParaInserirVM)
        {
            try
            {
                if (embarcadoraParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadora = _mapper.Map<EmbarcadoraTransportadora>(embarcadoraParaInserirVM);

                var EmbarcadoraIdInserido = await _service.InserirAsync(embarcadora);

                return CreatedAtAction(
                    actionName: nameof(ConsultarEmbarcadoraTransportadoraPeloId),
                    routeValues: new { id = EmbarcadoraIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Embarcadora criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}", Name = nameof(AtualizarEmbarcadoraTransportadora))]
        public async Task<IActionResult> AtualizarEmbarcadoraTransportadora(int id, [FromBody] EmbarcadoraTransportadoraParaAtualizarVM empresaParaAtualizarVM)
        {
            try
            {
                if (empresaParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadoraEncontrado = await _service.ConsultarPeloIdAsync(id);
                if (embarcadoraEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Registro não encontrado"));

                var embarcadora = _mapper.Map<EmbarcadoraTransportadora>(empresaParaAtualizarVM);
                var embarcadoraIdInserido = await _service.AtualizarAsync(embarcadora);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete("{id}", Name = nameof(ApagarEmbarcadoraTransportadora))]
        public async Task<IActionResult> ApagarEmbarcadoraTransportadora(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var embarcadoraEncontrado = await _service.ConsultarPeloIdAsync(id);
                if (embarcadoraEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Embarcadora não encontrada"));

                await _service.ApagarAsync(_mapper.Map<EmbarcadoraTransportadora>(embarcadoraEncontrado));

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
