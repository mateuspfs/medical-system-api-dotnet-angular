using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Helpers;
using SistemaMedico.Models;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TratamentoController(ITratamentoService tratamentoService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<TratamentoDTO>>>> PaginateAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var tratamentos = await tratamentoService.All(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<TratamentoDTO>>.SuccessResponse(tratamentos));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<TratamentoDTO>>.ErrorResponse($"Erro ao buscar tratamentos: {ex.Message}"));
            }
        }

        [HttpGet("filter")]
        public async Task<ActionResult<ApiResponse<PagedResult<TratamentoDTO>>>> Filter(string? filterNome, string? filterEspecialidade, int pageNumber, int pageSize)
        {
            try
            {
                var tratamentos = await tratamentoService.Filter(filterNome, filterEspecialidade, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<TratamentoDTO>>.SuccessResponse(tratamentos));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<TratamentoDTO>>.ErrorResponse($"Erro ao filtrar tratamentos: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TratamentoModel>>> Search(int id)
        {
            try
            {
                var tratamento = await tratamentoService.Search(id);
                return tratamento == null
                    ? NotFound(ApiResponse<TratamentoModel>.ErrorResponse($"Tratamento com ID {id} não encontrado."))
                    : Ok(ApiResponse<TratamentoModel>.SuccessResponse(tratamento));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TratamentoModel>.ErrorResponse($"Erro ao buscar tratamento: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TratamentoModel>>> Add(TratamentoModel tratamentoModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<TratamentoModel>.ErrorResponse("Dados inválidos", errors));
                }

                var novoTratamento = await tratamentoService.Add(tratamentoModel);
                return Ok(ApiResponse<TratamentoModel>.SuccessResponse(novoTratamento, "Tratamento criado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TratamentoModel>.ErrorResponse($"Erro ao criar tratamento: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TratamentoModel>>> Att(TratamentoModel tratamento, int id)
        {
            try
            {
                var tratamentoAtt = await tratamentoService.Att(tratamento, id);
                return Ok(ApiResponse<TratamentoModel>.SuccessResponse(tratamentoAtt, "Tratamento atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TratamentoModel>.ErrorResponse($"Erro ao atualizar tratamento: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await tratamentoService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Tratamento removido com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover tratamento."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover tratamento: {ex.Message}"));
            }
        }
    }
}
