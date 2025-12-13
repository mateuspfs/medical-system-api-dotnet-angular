using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaMedico.Data;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Repositories
{
    public class TratamentoRepository(SistemaMedicoDBContex dbContext) : ITratamentoRepository
    {

        public async Task<PagedResult<TratamentoDTO>> Filter(string? filterNome, string? filterEspecialidade, int pageNumber, int pageSize)
        {
            var query = dbContext.Tratamentos
                .Select(t => new TratamentoDTO
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Tempo = t.Tempo,
                    NomeEspecialidade = t.Especialidade.Nome,
                    EspecialidadeId = t.EspecialidadeId,
                    Etapas = t.Etapas.Select(et => new EtapaDTO { Id = et.Id, Titulo = et.Titulo, Descricao = et.Descricao }).ToList()
                });

            if (!string.IsNullOrEmpty(filterNome))
                query = query.Where(t => t.Nome.Contains(filterNome));;

            if (!string.IsNullOrEmpty(filterEspecialidade))
            {
                query = query.Where(t => t.NomeEspecialidade.Contains(filterEspecialidade));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedTratamentoes = await query
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<TratamentoDTO>
            {
                TotalPages = totalPages,
                Items = paginatedTratamentoes
            };

            return result;
        }

        public async Task<PagedResult<TratamentoDTO>> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var Tratamentos = await dbContext.Tratamentos
                            .Select(t => new TratamentoDTO
                            {
                                Id = t.Id,
                                Nome = t.Nome,
                                Tempo = t.Tempo,
                                NomeEspecialidade = t.Especialidade.Nome,
                                EspecialidadeId = t.EspecialidadeId,
                                Etapas = t.Etapas.Select(et => new EtapaDTO { Id = et.Id, Titulo = et.Titulo, Descricao = et.Descricao }).ToList()
                            })
                            .ToListAsync();

            var totalItems = Tratamentos.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedTratamento = Tratamentos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<TratamentoDTO>
            {
                TotalPages = totalPages,
                Items = paginatedTratamento
            };

            return result;
        }

        public async Task<TratamentoModel> Search(int id)
        {
            return await dbContext.Tratamentos
                .Include(e => e.Especialidade)
                .Include(et => et.Etapas)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TratamentoModel> Add(TratamentoModel tratamento)
        {
            var especialidade = await dbContext.Especialidades.FindAsync(tratamento.EspecialidadeId) ?? throw new ArgumentException("Especialidade não encontrada.");
            tratamento.Especialidade = especialidade;

            especialidade.Tratamentos ??= new List<TratamentoModel>();

            if (!especialidade.Tratamentos.Any(t => t.Id == tratamento.Id))
            {
                especialidade.Tratamentos.Add(tratamento);
            }

            await dbContext.Tratamentos.AddAsync(tratamento);
            await dbContext.SaveChangesAsync();

            return tratamento;
        }

        public async Task<TratamentoModel> Att(TratamentoModel tratamento, int id)
        {
            TratamentoModel TratamentoSearch = await Search(id) ?? throw new Exception($"Tratamento para o ID: {id} não foi encontrado no banco!");
           
            var especialidade = await dbContext.Especialidades.FindAsync(tratamento.EspecialidadeId) ?? throw new ArgumentException("Especialidade não encontrada.");

            var oldEspecialidade = await dbContext.Especialidades.FindAsync(TratamentoSearch.EspecialidadeId);
            oldEspecialidade?.Tratamentos.Remove(TratamentoSearch);

            especialidade.Tratamentos ??= new List<TratamentoModel>();

            TratamentoSearch.Nome = tratamento.Nome;
            TratamentoSearch.Tempo = tratamento.Tempo;
            TratamentoSearch.EspecialidadeId = tratamento.EspecialidadeId;
            TratamentoSearch.Especialidade = especialidade;

            if (!especialidade.Tratamentos.Any(t => t.Id == TratamentoSearch.Id))
            {
                especialidade.Tratamentos.Add(TratamentoSearch);
            }

            dbContext.Tratamentos.Update(TratamentoSearch);
            await dbContext.SaveChangesAsync();

            return TratamentoSearch;
        }

        public async Task<bool> Destroy(int id)
        {
            TratamentoModel TratamentoSearch = await Search(id) ?? throw new Exception($"Tratamento para o ID: {id} não foi encontrado no banco!");
            
            dbContext.Tratamentos.Remove(TratamentoSearch);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}

