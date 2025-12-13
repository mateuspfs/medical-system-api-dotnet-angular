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
    public class TratamentoPacienteController(ITratamentoPacienteService tratamentoPacienteService) : ControllerBase
    {

        [HttpGet("/DoutorPacientes")]
        public async Task<ActionResult<ApiResponse<PagedResult<TratamentoPacienteListDTO>>>> DoctorPacientes(string token, int pageNumber, int pageSize, string? search, string? filterTratamento)
        {
            try
            {
                var pacientes = await tratamentoPacienteService.DoctorPacientes(token, pageNumber, pageSize, search, filterTratamento);
                return Ok(ApiResponse<PagedResult<TratamentoPacienteListDTO>>.SuccessResponse(pacientes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<TratamentoPacienteListDTO>>.ErrorResponse($"Erro ao buscar pacientes do doutor: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TratamentoPacienteModel>>> Add(string token, [FromForm] TratamentoPacienteAddDTO tp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<TratamentoPacienteModel>.ErrorResponse("Dados inválidos", errors));
                }

                var novoTratamento = await tratamentoPacienteService.Add(token, tp);
                return Ok(ApiResponse<TratamentoPacienteModel>.SuccessResponse(novoTratamento, "Tratamento do paciente criado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TratamentoPacienteModel>.ErrorResponse($"Erro ao criar tratamento do paciente: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TratamentoPacienteModel>>> Att([FromForm] List<IFormFile> arquivos, int id, string token, int? novaEtapaId)
        {
            try
            {
                var tratamentoAtt = await tratamentoPacienteService.Att(arquivos, id, token, novaEtapaId);
                return Ok(ApiResponse<TratamentoPacienteModel>.SuccessResponse(tratamentoAtt, "Tratamento do paciente atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TratamentoPacienteModel>.ErrorResponse($"Erro ao atualizar tratamento do paciente: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await tratamentoPacienteService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Tratamento do paciente removido com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover tratamento do paciente."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover tratamento do paciente: {ex.Message}"));
            }
        }

        [HttpGet("/IntegraTratamentoPaciente/{tratamentoPacienteId}")]
        public async Task<ActionResult<ApiResponse<TratamentoPacienteViewDTO>>> GetTratamentoPaciente(string token, int tratamentoPacienteId)
        {
            try
            {
                var tratamento = await tratamentoPacienteService.GetTratamentoPaciente(token, tratamentoPacienteId);
                return Ok(ApiResponse<TratamentoPacienteViewDTO>.SuccessResponse(tratamento));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TratamentoPacienteViewDTO>.ErrorResponse($"Erro ao buscar tratamento do paciente: {ex.Message}"));
            }
        }

        [HttpGet("arquivo/{nomeArquivo}")]
        public async Task<ActionResult> GetArquivo(string nomeArquivo)
        {
            try
            {
                var fileStream = await tratamentoPacienteService.GetArquivo(nomeArquivo);
                if (fileStream == null)
                    return NotFound(ApiResponse<object?>.ErrorResponse("Arquivo não encontrado."));

                return File(fileStream, GetContentType(nomeArquivo), nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object?>.ErrorResponse($"Erro ao buscar arquivo: {ex.Message}"));
            }
        }

        private string GetContentType(string nomeArquivo)
        {
            var types = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".pdf", "application/pdf" },
            };

            var ext = Path.GetExtension(nomeArquivo).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}
