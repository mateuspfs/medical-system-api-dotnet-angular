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
    public class DoutorController(IDoutorService doutorService) : ControllerBase
    {

        [HttpGet("filter")]
        public async Task<ActionResult<ApiResponse<PagedResult<DoutorDTOView>>>> Filter(string? search, string? filterEspecialidade, int pageNumber, int pageSize)
        {
            try
            {
                var doutores = await doutorService.Filter(search, filterEspecialidade, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<DoutorDTOView>>.SuccessResponse(doutores));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<DoutorDTOView>>.ErrorResponse($"Erro ao filtrar doutores: {ex.Message}"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<DoutorDTOView>>>> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var doutores = await doutorService.All(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<DoutorDTOView>>.SuccessResponse(doutores));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<DoutorDTOView>>.ErrorResponse($"Erro ao buscar doutores: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DoutorDTOView>>> Search(int id)
        {
            try
            {
                var doutor = await doutorService.Search(id);
                return doutor == null 
                    ? NotFound(ApiResponse<DoutorDTOView>.ErrorResponse($"Doutor com ID {id} não encontrado."))
                    : Ok(ApiResponse<DoutorDTOView>.SuccessResponse(doutor));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DoutorDTOView>.ErrorResponse($"Erro ao buscar doutor: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DoutorModel>>> Add([FromForm] DoutorDTO doutorModel, [FromQuery] string especialidadeIds)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<DoutorModel>.ErrorResponse("Dados inválidos", errors));
                }

                var novoDoutor = await doutorService.Add(doutorModel, especialidadeIds);
                return Ok(ApiResponse<DoutorModel>.SuccessResponse(novoDoutor, "Doutor criado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DoutorModel>.ErrorResponse($"Erro ao criar doutor: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DoutorModel>>> Att([FromForm] DoutorDTO doutor, int id, [FromQuery] string especialidadeIds)
        {
            try
            {
                var doutorAtt = await doutorService.Att(doutor, id, especialidadeIds);
                return Ok(ApiResponse<DoutorModel>.SuccessResponse(doutorAtt, "Doutor atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DoutorModel>.ErrorResponse($"Erro ao atualizar doutor: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await doutorService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Doutor removido com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover doutor."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover doutor: {ex.Message}"));
            }
        }

        [HttpGet("TratamentosDoutor")]
        public async Task<ActionResult<ApiResponse<PagedResult<TratamentoDTO>>>> TratamentosDoutor(string token, string? filterNome, int? filterEspecialidade, int pageNumber, int pageSize)
        {
            try
            {
                var tratamentos = await doutorService.TratamentosDoutor(token, filterNome, filterEspecialidade, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<TratamentoDTO>>.SuccessResponse(tratamentos));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<TratamentoDTO>>.ErrorResponse($"Erro ao buscar tratamentos: {ex.Message}"));
            }
        }

        [HttpGet("EspecialidadesDoutor")]
        public async Task<ActionResult<ApiResponse<List<EspecialidadeDTO>>>> EspecialidadesDoutor(string token)
        {
            try
            {
                var especialidades = await doutorService.EspecialidadesDoutor(token);
                return Ok(ApiResponse<List<EspecialidadeDTO>>.SuccessResponse(especialidades));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<EspecialidadeDTO>>.ErrorResponse($"Erro ao buscar especialidades: {ex.Message}"));
            }
        }

        [HttpGet("imagem/{nomeArquivo}")]
        public async Task<ActionResult> GetImagem(string nomeArquivo)
        {
            try
            {
                var fileStream = await doutorService.GetImagem(nomeArquivo);
                if (fileStream == null)
                    return NotFound(ApiResponse<object?>.ErrorResponse("Imagem não encontrada."));

                return new FileStreamResult(fileStream, GetContentType(nomeArquivo));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object?>.ErrorResponse($"Erro ao buscar imagem: {ex.Message}"));
            }
        }

        private string GetContentType(string nomeArquivo)
        {
            var types = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
            };

            var ext = Path.GetExtension(nomeArquivo).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}
