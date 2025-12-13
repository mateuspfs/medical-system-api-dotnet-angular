using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Helpers;
using SistemaMedico.Models;
using SistemaMedico.Services.Interfaces;

namespace SistemaMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtapaController(IEtapaService etapaService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EtapaModel>>>> All()
        {
            try
            {
                var etapas = await etapaService.All();
                return Ok(ApiResponse<List<EtapaModel>>.SuccessResponse(etapas));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<EtapaModel>>.ErrorResponse($"Erro ao buscar etapas: {ex.Message}"));
            }
        }

        [HttpGet("Pacientes")]
        public async Task<ActionResult<ApiResponse<EtapaPacienteDTO>>> PacientesEtapa(string? search, int pageNumber, int pageSize, int id)
        {
            try
            {
                var result = await etapaService.PacientesEtapa(search, pageNumber, pageSize, id);
                return Ok(ApiResponse<EtapaPacienteDTO>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EtapaPacienteDTO>.ErrorResponse($"Erro ao buscar pacientes da etapa: {ex.Message}"));
            }
        }

        [HttpGet("tratamento/{id}")]
        public async Task<ActionResult<ApiResponse<EtapaTratamentoDTO>>> SearchTratamento(int id)
        {
            try
            {
                var etapas = await etapaService.SearchTratamento(id);
                return Ok(ApiResponse<EtapaTratamentoDTO>.SuccessResponse(etapas));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EtapaTratamentoDTO>.ErrorResponse($"Erro ao buscar etapas do tratamento: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EtapaModel>>> Search(int id)
        {
            try
            {
                var etapa = await etapaService.Search(id);
                return etapa == null
                    ? NotFound(ApiResponse<EtapaModel>.ErrorResponse($"Etapa com ID {id} não encontrada."))
                    : Ok(ApiResponse<EtapaModel>.SuccessResponse(etapa));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EtapaModel>.ErrorResponse($"Erro ao buscar etapa: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EtapaModel>>> Add(EtapaModel etapa)
        {
            try
            {
                var novaEtapa = await etapaService.Add(etapa);
                return Ok(ApiResponse<EtapaModel>.SuccessResponse(novaEtapa, "Etapa criada com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EtapaModel>.ErrorResponse($"Erro ao criar etapa: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EtapaModel>>> Att(EtapaModel etapa, int id)
        {
            try
            {
                var etapaAtt = await etapaService.Att(etapa, id);
                return Ok(ApiResponse<EtapaModel>.SuccessResponse(etapaAtt, "Etapa atualizada com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EtapaModel>.ErrorResponse($"Erro ao atualizar etapa: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await etapaService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Etapa removida com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover etapa."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover etapa: {ex.Message}"));
            }
        }
    }
}
