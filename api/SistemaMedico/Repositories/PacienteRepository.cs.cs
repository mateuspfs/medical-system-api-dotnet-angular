using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaMedico.Data;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Utilies;
using System.Text.RegularExpressions;

namespace SistemaMedico.Repositories
{
    public class PacienteRepository(SistemaMedicoDBContex dbContext) : IPacienteRepository
    {

        public async Task<PagedResult<PacienteDTO>> Filter(string? search, int pageNumber, int pageSize)
        {
            // Construa a consulta base
            var query = dbContext.Pacientes
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

            // Aplicar filtros conforme necessário
            if (!string.IsNullOrEmpty(search))
            {
                if (search.Any(char.IsLetter))
                {
                    query = query.Where(d => d.Nome.Contains(search) || d.Email.Contains(search) || d.Codigo.Contains(search));
                }
                else
                {
                    search = Regex.Replace(search, "[^0-9]", "");
                    query = query.Where(d => d.Cpf.Contains(search) || d.Telefone.Contains(search));
                }
            }

            // Paginação
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedPacientes = await query
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<PacienteDTO>
            {
                TotalPages = totalPages,
                Items = paginatedPacientes
            };

            return result;
        }

        public async Task<PagedResult<PacienteModel>> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var Pacientes = await dbContext.Pacientes
                                        .Include(tp => tp.TratamentoPaciente)
                                        .ToListAsync();

            var totalItems = Pacientes.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedPaciente = Pacientes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<PacienteModel>
            {
                TotalPages = totalPages,
                Items = paginatedPaciente
            };

            return result;
        }

        public async Task<PacienteModel> Search(int id)
        {
            return await dbContext.Pacientes.Include(tp => tp.TratamentoPaciente).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PacienteModel> Add(PacienteModel Paciente)
        {
            Paciente.Codigo = GenerateRandomCode();
            Paciente.Telefone = Regex.Replace(Paciente.Telefone, "[^0-9]", "");
            Paciente.Cpf = Regex.Replace(Paciente.Cpf, "[^0-9]", "");

            if (!await ValidateUniqueFields(Paciente.Email, Paciente.Cpf))
            {
                throw new ArgumentException("Já existe um doutor com o mesmo email ou CPF.");
            }

            while (await dbContext.Pacientes.AnyAsync(p => p.Codigo == Paciente.Codigo))
            {
                Paciente.Codigo = GenerateRandomCode();
            }

            await dbContext.Pacientes.AddAsync(Paciente);
            await dbContext.SaveChangesAsync();
            return Paciente;
        }

        public async Task<PacienteModel> Att(PacienteModel Paciente, int id)
        {
            var pacienteOld = await Search(id) ?? throw new Exception($"Paciente para o ID: {id} não foi encontrado no banco!");

            pacienteOld.Nome = Paciente.Nome;
            pacienteOld.Email = Paciente.Email;
            pacienteOld.Telefone = Regex.Replace(Paciente.Telefone, "[^0-9]", "");
            pacienteOld.Endereco = Paciente.Endereco;

            dbContext.Pacientes.Update(pacienteOld);
            await dbContext.SaveChangesAsync();

            return pacienteOld;
        }

        public async Task<bool> Destroy(int id)
        {
            PacienteModel PacienteSearch = await Search(id) ?? throw new Exception($"Paciente para o ID: {id} não foi encontrado no banco!");
           
            dbContext.Pacientes.Remove(PacienteSearch);
            await dbContext.SaveChangesAsync();
            return true;
        }
        private string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> ValidateUniqueFields(string email, string cpf)
        {
            var existingPaciente = await dbContext.Pacientes
                .FirstOrDefaultAsync(d => d.Email == email || d.Cpf == cpf);

            return existingPaciente == null;
        }
    }
}
