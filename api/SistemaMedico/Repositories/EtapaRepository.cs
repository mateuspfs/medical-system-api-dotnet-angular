using Microsoft.EntityFrameworkCore;
using SistemaMedico.Data;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Utilies;
using System.Text.RegularExpressions;

namespace SistemaMedico.Repositories
{
    public class EtapaRepository(SistemaMedicoDBContex dbContext) : IEtapaRepository
    {

        public async Task<List<EtapaModel>> All()
        {
            return await dbContext.Etapas
                .Include(t => t.Tratamento)
                .Include(tp => tp.TratamentoPaciente)
                .ToListAsync();
        }
        public async Task<EtapaModel> Search(int id)
        {
            return await dbContext.Etapas
                .Include(t => t.Tratamento)
                .Include(tp => tp.TratamentoPaciente)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<EtapaTratamentoDTO> SearchTratamento(int id)
        {
            var tratamentoEtapas = await dbContext.Etapas
                                .Where(e => e.TratamentoId == id)
                                .OrderBy(e => e.Numero)
                                .ToListAsync();

            var etapasCount = tratamentoEtapas.Count;
            var orderedEtapas = tratamentoEtapas.OrderBy(e => e.Numero).ToList();

            var tratamento = await dbContext.Tratamentos.FindAsync(id);

            var etapaDTOs = orderedEtapas.Select(e => new EtapaDTO
            {
                Id = e.Id,
                Titulo = e.Titulo,
                Descricao = e.Descricao,
                Numero = e.Numero,
            }).ToList();

            return new EtapaTratamentoDTO
            {
                TratamentoId = tratamento.Id,
                TratamentoNome = tratamento.Nome,
                Count = etapasCount,
                Etapas = etapaDTOs
            };
        }

        public async Task<EtapaModel> Add(EtapaModel Etapa)
        {
            var lastEtapa = await dbContext.Etapas
                .Where(e => e.TratamentoId == Etapa.TratamentoId)
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync();

            Etapa.Numero = lastEtapa != null ? lastEtapa.Numero + 1 : 1;

            await dbContext.Etapas.AddAsync(Etapa);
            await dbContext.SaveChangesAsync();

            var Tratamento = await dbContext.Tratamentos.FindAsync(Etapa.TratamentoId) ?? throw new ArgumentException("Tratamento não encontrada.");
            Etapa.Tratamento = Tratamento;

            Tratamento.Etapas ??= new List<EtapaModel>();

            if (!Tratamento.Etapas.Any(t => t.Id == Etapa.Id))
            {
                Tratamento.Etapas.Add(Etapa);
            }

            return Etapa;
        }

        public async Task<EtapaModel> Att(EtapaModel Etapa, int id)
        {
            EtapaModel EtapaSearch = await Search(id) ?? throw new Exception($"Etapa com o ID: {id} não foi encontrada no banco!");

            EtapaSearch.Titulo = Etapa.Titulo;
            EtapaSearch.Descricao = Etapa.Descricao;
            EtapaSearch.TratamentoId = Etapa.TratamentoId;

            var Tratamento = await dbContext.Tratamentos.FindAsync(Etapa.TratamentoId) ?? throw new ArgumentException("Tratamento não encontrado.");

            Tratamento.Etapas ??= new List<EtapaModel>();

            if (!Tratamento.Etapas.Any(t => t.Id == Etapa.Id))
            {
                Tratamento.Etapas.Add(EtapaSearch);
            }

            dbContext.Etapas.Update(EtapaSearch);
            await dbContext.SaveChangesAsync();
            return EtapaSearch;
        }


        public async Task<bool> Destroy(int id)
        {
            EtapaModel EtapaSearch = await Search(id) ?? throw new Exception($"Doutor para o ID: {id} não foi encontrado no banco!");

            dbContext.Etapas.Remove(EtapaSearch);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<EtapaPacienteDTO> PacientesEtapa(string? search, int pageNumber, int pageSize, int id)
        {
            var etapa = await dbContext.Etapas
                .Include(t => t.Tratamento)
                .FirstOrDefaultAsync(x => x.Id == id);

            var queryPacientes = dbContext.Pacientes
                .Where(p => p.TratamentoPaciente.Any(tp => tp.EtapaId == id)) 
                .Select(d => new PacienteDTO
                {
                    Id = d.Id,
                    Codigo = d.Codigo,
                    Email = d.Email,
                    Telefone = d.Telefone,
                    Nome = d.Nome,
                    Cpf = d.Cpf,
                    Endereco = d.Endereco
                });

            if (!string.IsNullOrEmpty(search))
            {
                if (search.Any(char.IsLetter))
                {
                    queryPacientes = queryPacientes.Where(d => d.Nome.Contains(search) || d.Email.Contains(search) || d.Codigo.Contains(search));
                }
                else
                {
                    search = Regex.Replace(search, "[^0-9]", "");
                    queryPacientes = queryPacientes.Where(d => d.Cpf.Contains(search) || d.Telefone.Contains(search));
                }
            }

            var totalItems = await queryPacientes.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var queryPacientesResult = await queryPacientes
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatePacientes = new PagedResult<PacienteDTO>
            {
                TotalPages = totalPages,
                Items = queryPacientesResult
            };

            return new EtapaPacienteDTO
            {
                Titulo = etapa.Titulo,
                Tratamento = etapa.Tratamento.Nome,
                Descricao = etapa.Descricao,
                Pacientes = paginatePacientes
            };
        }
    }
}
