using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services.Interfaces
{
    public interface IDoutorService
    {
        Task<PagedResult<DoutorDTOView>> Filter(string? search, string? filterEspecialidade, int pageNumber, int pageSize);
        Task<PagedResult<DoutorDTOView>> All(int pageNumber, int pageSize);
        Task<DoutorDTOView?> Search(int id);
        Task<DoutorModel> Add([FromForm] DoutorDTO doutor, [FromQuery] string especialidadeIds);
        Task<DoutorModel> Att([FromForm] DoutorDTO doutor, int id, [FromQuery] string especialidadeIds);
        Task<bool> Destroy(int id);
        Task<List<EspecialidadeDTO>> EspecialidadesDoutor(string token);
        Task<PagedResult<TratamentoDTO>> TratamentosDoutor(string token, string? filterNome, int? filterEspecialidade, int pageNumber, int pageSize);
        Task<FileStream?> GetImagem(string nomeArquivo);
    }
}

