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
    public class PagamentoController(IPagamentoService pagamentoService) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PagamentoEtapaModel>>> Search(int id)
        {
            try
            {
                var pagamento = await pagamentoService.Search(id);
                return pagamento == null
                    ? NotFound(ApiResponse<PagamentoEtapaModel>.ErrorResponse($"Pagamento com ID {id} não encontrado."))
                    : Ok(ApiResponse<PagamentoEtapaModel>.SuccessResponse(pagamento));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagamentoEtapaModel>.ErrorResponse($"Erro ao buscar pagamento: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PagamentoResponse>>> Add(string token, PagamentoEtapaDTO pagamentoDTO, int tratamentoId)
        {
            try
            {
                var response = await pagamentoService.Add(token, pagamentoDTO, tratamentoId);
                return Ok(ApiResponse<PagamentoResponse>.SuccessResponse(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagamentoResponse>.ErrorResponse($"Erro ao processar pagamento: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PagamentoEtapaModel>>> Att(PagamentoEtapaModel pagamento, int id)
        {
            try
            {
                var pagamentoAtt = await pagamentoService.Att(pagamento, id);
                return Ok(ApiResponse<PagamentoEtapaModel>.SuccessResponse(pagamentoAtt, "Pagamento atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagamentoEtapaModel>.ErrorResponse($"Erro ao atualizar pagamento: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await pagamentoService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Pagamento removido com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover pagamento."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover pagamento: {ex.Message}"));
            }
        }
    }
}
