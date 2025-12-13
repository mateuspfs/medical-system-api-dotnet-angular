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
    public class EspecialidadeController(IEspecialidadeService especialidadeService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EspecialidadeModel>>>> All()
        {
            try
            {
                var especialidades = await especialidadeService.All();
                return Ok(ApiResponse<List<EspecialidadeModel>>.SuccessResponse(especialidades));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<EspecialidadeModel>>.ErrorResponse($"Erro ao buscar especialidades: {ex.Message}"));
            }
        } 

        [HttpGet("filter")]
        public async Task<ActionResult<ApiResponse<PagedResult<EspecialidadeDTO>>>> Filter(string? search, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var especialidades = await especialidadeService.Filter(search, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<EspecialidadeDTO>>.SuccessResponse(especialidades));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<EspecialidadeDTO>>.ErrorResponse($"Erro ao filtrar especialidades: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EspecialidadeModel>>> Search(int id)
        {
            try
            {
                var especialidade = await especialidadeService.Search(id);
                return especialidade == null
                    ? NotFound(ApiResponse<EspecialidadeModel>.ErrorResponse($"Especialidade com ID {id} não encontrada."))
                    : Ok(ApiResponse<EspecialidadeModel>.SuccessResponse(especialidade));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EspecialidadeModel>.ErrorResponse($"Erro ao buscar especialidade: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EspecialidadeModel>>> Add(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                    return BadRequest(ApiResponse<EspecialidadeModel>.ErrorResponse("Nome da especialidade é obrigatório."));

                var novaEspecialidade = await especialidadeService.Add(nome);
                return Ok(ApiResponse<EspecialidadeModel>.SuccessResponse(novaEspecialidade, "Especialidade criada com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EspecialidadeModel>.ErrorResponse($"Erro ao criar especialidade: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EspecialidadeModel>>> Att(string nome, int id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                    return BadRequest(ApiResponse<EspecialidadeModel>.ErrorResponse("Nome da especialidade é obrigatório."));

                var especialidadeAtt = await especialidadeService.Att(nome, id);
                return Ok(ApiResponse<EspecialidadeModel>.SuccessResponse(especialidadeAtt, "Especialidade atualizada com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EspecialidadeModel>.ErrorResponse($"Erro ao atualizar especialidade: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await especialidadeService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Especialidade removida com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover especialidade."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover especialidade: {ex.Message}"));
            }
        }
    }
}
