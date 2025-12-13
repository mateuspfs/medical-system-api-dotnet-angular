using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services.Interfaces
{
    public interface ITratamentoService
    {
        Task<PagedResult<TratamentoDTO>> Filter(string? filterNome, string? filterEspecialidade, int pageNumber, int pageSize);
        Task<PagedResult<TratamentoDTO>> All(int pageNumber, int pageSize);
        Task<TratamentoModel> Search(int id);
        Task<TratamentoModel> Add(TratamentoModel tratamento);
        Task<TratamentoModel> Att(TratamentoModel tratamento, int id);
        Task<bool> Destroy(int id);
    }
}

