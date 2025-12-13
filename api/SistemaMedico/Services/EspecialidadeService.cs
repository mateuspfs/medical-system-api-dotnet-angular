using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class EspecialidadeService(IEspecialidadeRepository especialidadeRepository) : IEspecialidadeService
    {

        public async Task<List<EspecialidadeModel>> All()
        {
            return await especialidadeRepository.All();
        }

        public async Task<PagedResult<EspecialidadeDTO>> PaginateAll(int pageNumber, int pageSize)
        {
            return await especialidadeRepository.paginateAll(pageNumber, pageSize);
        }

        public async Task<PagedResult<EspecialidadeDTO>> Filter(string? search, int pageNumber, int pageSize)
        {
            return await especialidadeRepository.Filter(search, pageNumber, pageSize);
        }

        public async Task<EspecialidadeModel> Search(int id)
        {
            return await especialidadeRepository.Search(id);
        }

        public async Task<EspecialidadeModel> Add(string nome)
        {
            return await especialidadeRepository.Add(nome);
        }

        public async Task<EspecialidadeModel> Att(string nome, int id)
        {
            return await especialidadeRepository.Att(nome, id);
        }

        public async Task<bool> Destroy(int id)
        {
            return await especialidadeRepository.Destroy(id);
        }
    }
}

