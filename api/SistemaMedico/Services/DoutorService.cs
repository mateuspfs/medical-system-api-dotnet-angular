using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class DoutorService(IDoutorRepository doutorRepository) : IDoutorService
    {

        public async Task<PagedResult<DoutorDTOView>> Filter(string? search, string? filterEspecialidade, int pageNumber, int pageSize)
        {
            return await doutorRepository.Filter(search, filterEspecialidade, pageNumber, pageSize);
        }

        public async Task<PagedResult<DoutorDTOView>> All(int pageNumber, int pageSize)
        {
            return await doutorRepository.All(pageNumber, pageSize);
        }

        public async Task<DoutorDTOView?> Search(int id)
        {
            return await doutorRepository.Search(id);
        }

        public async Task<DoutorModel> Add([FromForm] DoutorDTO doutor, [FromQuery] string especialidadeIds)
        {
            return await doutorRepository.Add(doutor, especialidadeIds);
        }

        public async Task<DoutorModel> Att([FromForm] DoutorDTO doutor, int id, [FromQuery] string especialidadeIds)
        {
            return await doutorRepository.Att(doutor, id, especialidadeIds);
        }

        public async Task<bool> Destroy(int id)
        {
            return await doutorRepository.Destroy(id);
        }

        public async Task<List<EspecialidadeDTO>> EspecialidadesDoutor(string token)
        {
            return await doutorRepository.EspecialidadesDoutor(token);
        }

        public async Task<PagedResult<TratamentoDTO>> TratamentosDoutor(string token, string? filterNome, int? filterEspecialidade, int pageNumber, int pageSize)
        {
            return await doutorRepository.TratamentosDoutor(token, filterNome, filterEspecialidade, pageNumber, pageSize);
        }

        public async Task<FileStream?> GetImagem(string nomeArquivo)
        {
            return await doutorRepository.GetImagem(nomeArquivo);
        }
    }
}

