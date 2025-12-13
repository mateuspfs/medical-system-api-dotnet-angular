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
    public class PacienteController(IPacienteService pacienteService) : ControllerBase
    {

        [HttpGet("filter")]
        public async Task<ActionResult<ApiResponse<PagedResult<PacienteDTO>>>> Filter(string? search, int pageNumber, int pageSize)
        {
            try
            {
                var pacientes = await pacienteService.Filter(search, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<PacienteDTO>>.SuccessResponse(pacientes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<PacienteDTO>>.ErrorResponse($"Erro ao filtrar pacientes: {ex.Message}"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<PacienteModel>>>> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var pacientes = await pacienteService.All(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<PacienteModel>>.SuccessResponse(pacientes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<PacienteModel>>.ErrorResponse($"Erro ao buscar pacientes: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PacienteModel>>> Search(int id)
        {
            try
            {
                var paciente = await pacienteService.Search(id);
                return paciente == null
                    ? NotFound(ApiResponse<PacienteModel>.ErrorResponse($"Paciente com ID {id} não encontrado."))
                    : Ok(ApiResponse<PacienteModel>.SuccessResponse(paciente));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PacienteModel>.ErrorResponse($"Erro ao buscar paciente: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PacienteModel>>> Add(PacienteModel pacienteModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<PacienteModel>.ErrorResponse("Dados inválidos", errors));
                }

                var novoPaciente = await pacienteService.Add(pacienteModel);
                return Ok(ApiResponse<PacienteModel>.SuccessResponse(novoPaciente, "Paciente criado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PacienteModel>.ErrorResponse($"Erro ao criar paciente: {ex.Message}"));
            }
        }         
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PacienteModel>>> Att(PacienteModel paciente, int id)
        {
            try
            {
                var pacienteAtt = await pacienteService.Att(paciente, id);
                return Ok(ApiResponse<PacienteModel>.SuccessResponse(pacienteAtt, "Paciente atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PacienteModel>.ErrorResponse($"Erro ao atualizar paciente: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await pacienteService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Paciente removido com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover paciente."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover paciente: {ex.Message}"));
            }
        }
    }
}
