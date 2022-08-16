using AutoMapper;
using ItAccept.Teste.Application.Attributes;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Domain.ViewModels.Produtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItAccept.Teste.Application.Controllers.v1
{
    [ValidateModel]
    [Authorize("Bearer")]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/produtos")]
    public class ProdutosController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProdutosService _produtosService;
        private readonly IOfertasService _ofertasService;

        public ProdutosController(IMapper mapper, IOfertasService ofertasService,
            IProdutosService produtosService, ILogger<ProdutosController> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _produtosService = produtosService ?? throw new ArgumentNullException(nameof(produtosService));
            _ofertasService = ofertasService ?? throw new ArgumentNullException(nameof(ofertasService));
        }

        [HttpGet("{id}", Name = nameof(ConsultarProdutoPeloId))]
        public async Task<IActionResult> ConsultarProdutoPeloId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var produtoEncontrado = await _produtosService.ConsultarPeloIdAsync(id);
                if (produtoEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Produto não encontrado"));

                return Ok(new ApiResponse(ApiResponseState.Success, "", produtoEncontrado));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost(Name = nameof(InserirProduto))]
        public async Task<IActionResult> InserirProduto([FromBody] ProdutoParaInserirVM produtoParaInserirVM)
        {
            try
            {
                if (produtoParaInserirVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var produto = _mapper.Map<Produto>(produtoParaInserirVM);
                produto.Status = true;

                var produtoIdInserido = await _produtosService.InserirAsync(produto);

                return CreatedAtAction(
                    actionName: nameof(ConsultarProdutoPeloId),
                    routeValues: new { id = produtoIdInserido, version = ApiVersion.Default.MajorVersion?.ToString() },
                    value: new ApiResponse(ApiResponseState.Success, "Produto criado com sucesso"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}", Name = nameof(AtualizarProduto))]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] ProdutoParaAtualizarVM produtoParaAtualizarVM)
        {
            try
            {
                if (produtoParaAtualizarVM is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var produtoEncontrado = await _produtosService.ConsultarPeloIdAsync(id);
                if (produtoEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Produto não encontrado"));

                var produto = _mapper.Map<Produto>(produtoParaAtualizarVM);
                
                var produtoIdInserido = await _produtosService.AtualizarAsync(produto);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}/status", Name = nameof(AtualizarStatusProduto))]
        public async Task<IActionResult> AtualizarStatusProduto(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var produtoEncontrado = await _produtosService.ConsultarPeloIdAsync(id);
                if (produtoEncontrado is null)
                    return NotFound(new ApiResponse(ApiResponseState.Failed, "Produto não encontrado"));

                produtoEncontrado.Status = !produtoEncontrado.Status;
                await _produtosService.InativarAsync(_mapper.Map<Produto>(produtoEncontrado));

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}/ofertas", Name = nameof(ConsultarOfertasPeloProduto))]
        public async Task<IActionResult> ConsultarOfertasPeloProduto(int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Request inválido"));

                var produto = await _produtosService.ConsultarPeloIdAsync(id);
                if (produto is null)
                    return BadRequest(new ApiResponse(ApiResponseState.Failed, "Produto não encontrado"));

                var ofertas = status switch
                {
                    "ativos" => await _ofertasService.ConsultarAtivosPeloProdutoAsync(id),
                    "inativos" => (await _ofertasService.ConsultarPeloProdutoAsync(id)).Where(x => !x.Status),
                    _ => await _ofertasService.ConsultarPeloProdutoAsync(id)
                };

                return Ok(new ApiResponse(ApiResponseState.Success, "", ofertas));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
