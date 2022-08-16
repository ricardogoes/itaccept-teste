using AutoMapper;
using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Domain.ViewModels.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers.v1
{
    [ValidateModel]
    [Authorize("Bearer")]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuarios")]
    public class UsuariosController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IMapper mapper,
            IUsuariosService usuariosService, ILogger<UsuariosController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usuariosService = usuariosService ?? throw new ArgumentNullException(nameof(usuariosService));
        }

        [HttpGet("{id}", Name = nameof(ConsultarUsuarioPeloId))]
        public async Task<IActionResult> ConsultarUsuarioPeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var usuarioEncontrado = await _usuariosService.ConsultarPeloIdAsync(id);
                if (usuarioEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Usuario não encontrado"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", usuarioEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirUsuario))]
        public async Task<IActionResult> InserirUsuario([FromBody] UsuarioParaInserirVM usuarioParaInserirVM)
        {
            try
            {
                if (usuarioParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var usuario = _mapper.Map<Usuario>(usuarioParaInserirVM);
                usuario.Status = true;

                var usuarioIdInserido = await _usuariosService.InserirAsync(usuario);

                return CreatedAtAction(
                    actionName: nameof(ConsultarUsuarioPeloId),
                    routeValues: new { id = usuarioIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Usuario criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}", Name = nameof(AtualizarUsuario))]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] UsuarioParaAtualizarVM usuarioParaAtualizarVM)
        {
            try
            {
                if (usuarioParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var usuarioEncontrado = await _usuariosService.ConsultarPeloIdAsync(id);
                if (usuarioEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Usuario não encontrado"));

                var usuario = _mapper.Map<Usuario>(usuarioParaAtualizarVM);
                usuario.Status = true;

                var usuarioIdInserido = await _usuariosService.AtualizarAsync(usuario);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/status", Name = nameof(InativarUsuario))]
        public async Task<IActionResult> InativarUsuario(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var usuarioEncontrado = await _usuariosService.ConsultarPeloIdAsync(id);
                if (usuarioEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Usuario não encontrado"));

                usuarioEncontrado.Status = !usuarioEncontrado.Status;
                await _usuariosService.InativarAsync(_mapper.Map<Usuario>(usuarioEncontrado));

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
