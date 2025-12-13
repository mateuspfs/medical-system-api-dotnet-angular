using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class TratamentoService(ITratamentoRepository tratamentoRepository) : ITratamentoService
    {

        public async Task<PagedResult<TratamentoDTO>> Filter(string? filterNome, string? filterEspecialidade, int pageNumber, int pageSize)
        {
            return await tratamentoRepository.Filter(filterNome, filterEspecialidade, pageNumber, pageSize);
        }

        public async Task<PagedResult<TratamentoDTO>> All(int pageNumber, int pageSize)
        {
            return await tratamentoRepository.All(pageNumber, pageSize);
        }

        public async Task<TratamentoModel> Search(int id)
        {
            return await tratamentoRepository.Search(id);
        }

        public async Task<TratamentoModel> Add(TratamentoModel tratamento)
        {
            return await tratamentoRepository.Add(tratamento);
        }

        public async Task<TratamentoModel> Att(TratamentoModel tratamento, int id)
        {
            return await tratamentoRepository.Att(tratamento, id);
        }

        public async Task<bool> Destroy(int id)
        {
            return await tratamentoRepository.Destroy(id);
        }
    }
}

