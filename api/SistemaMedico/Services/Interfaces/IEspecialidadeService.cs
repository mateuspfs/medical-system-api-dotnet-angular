using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services.Interfaces
{
    public interface IEspecialidadeService
    {
        Task<List<EspecialidadeModel>> All();
        Task<PagedResult<EspecialidadeDTO>> PaginateAll(int pageNumber, int pageSize);
        Task<PagedResult<EspecialidadeDTO>> Filter(string? search, int pageNumber, int pageSize);
        Task<EspecialidadeModel> Search(int id);
        Task<EspecialidadeModel> Add(string nome);
        Task<EspecialidadeModel> Att(string nome, int id);
        Task<bool> Destroy(int id);
    }
}

