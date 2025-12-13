using Microsoft.AspNetCore.Components.Routing;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SistemaMedico.Data;
using SistemaMedico.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SistemaMedico.Utilies;
using SistemaMedico.DTOs;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SistemaMedico.Migrations;
using System.Text.RegularExpressions;
using SistemaMedico.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace SistemaMedico.Repositories
{   
    public class DoutorRepository(SistemaMedicoDBContex dbContext, IMapper mapper, IConfiguration configuration) : IDoutorRepository
    {
        private readonly string _jwtSecret = configuration["Jwt:Secret"] ?? string.Empty;

        public async Task<PagedResult<DoutorDTOView>> Filter(string? search, string? filterEspecialidade, int pageNumber, int pageSize)
        {
            // Construa a consulta base
            var query = dbContext.Doutores
                .Include(d => d.DoutorEspecialidades)
                    .ThenInclude(de => de.Especialidade)
                .Select(d => new DoutorDTOView
                {
                    Id = d.Id,
                    Email = d.Email,
                    Telefone = d.Telefone,
                    Nome = d.Nome,
                    Cpf = d.Cpf,
                    DocumentoNome = d.DocumentoNome,
                    Endereco = d.Endereco,
                    Especialidades = string.Join(", ", d.DoutorEspecialidades.Select(de => de.Especialidade.Nome))
                });

            // Aplicar filtros conforme necessário
            if (!string.IsNullOrEmpty(search))
            {
                if(search.Any(char.IsLetter))
                {
                    query = query.Where(d => d.Nome.Contains(search) || d.Email.Contains(search));
                } else
                {
                    search = Regex.Replace(search, "[^0-9]", "");
                    query = query.Where(d => d.Cpf.Contains(search) || d.Telefone.Contains(search));
                }
            }

            // Paginação
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedDoutores = await query
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!string.IsNullOrEmpty(filterEspecialidade))
            {
                paginatedDoutores = paginatedDoutores.Where(d => d.Especialidades.Contains(filterEspecialidade)).ToList();
            }

            var result = new PagedResult<DoutorDTOView>
            {
                TotalPages = totalPages,
                Items = paginatedDoutores
            };

            return result;
        }

        public async Task<PagedResult<DoutorDTOView>> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var Doutores = await dbContext.Doutores
                                .Include(d => d.DoutorEspecialidades)
                                    .ThenInclude(de => de.Especialidade)
                                .Select(d => new DoutorDTOView
                                {
                                    Id = d.Id,
                                    Email = d.Email,
                                    Telefone = d.Telefone,
                                    Nome = d.Nome,
                                    Cpf = d.Cpf,
                                    DocumentoNome = d.DocumentoNome,
                                    Endereco = d.Endereco,
                                    Especialidades = string.Join(", ", d.DoutorEspecialidades.Select(de => de.Especialidade.Nome))
                                })
                                .ToListAsync();

            var totalItems = Doutores.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedDoutores = Doutores
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<DoutorDTOView>
            {
                TotalPages = totalPages,
                Items = paginatedDoutores
            };

            return result;
        }
        
        public async Task<DoutorDTOView> Search(int id)
        {
            return await dbContext.Doutores
                                .Include(d => d.DoutorEspecialidades)
                                    .ThenInclude(de => de.Especialidade)
                                .Select(d => new DoutorDTOView
                                {
                                    Id = d.Id,
                                    Email = d.Email,
                                    Telefone = d.Telefone,
                                    Nome = d.Nome,
                                    Cpf = d.Cpf,
                                    DocumentoNome = d.DocumentoNome,
                                    Endereco = d.Endereco,
                                    Especialidades = string.Join(", ", d.DoutorEspecialidades.Select(de => de.Especialidade.Nome))
                                })
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DoutorModel> Add([FromForm] DoutorDTO Doctor, [FromQuery] string especialidadeIds)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Doctor.Documento.FileName);
            var filePath = Path.Combine("Storage/DoutoresDocs", uniqueFileName);

            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            Doctor.Documento.CopyTo(fileStream);

            Doctor.DocumentoNome = uniqueFileName;
            Doctor.Cpf = Regex.Replace(Doctor.Cpf, "[^0-9]", ""); 
            Doctor.Telefone = Regex.Replace(Doctor.Telefone, "[^0-9]", ""); 

            var doutorModel = mapper.Map<DoutorModel>(Doctor);

            await UpdateDoutorEspecialidades(doutorModel, especialidadeIds);
            await dbContext.Doutores.AddAsync(doutorModel);
            await dbContext.SaveChangesAsync();

            return doutorModel;
        }

        public async Task<DoutorModel> Att([FromForm] DoutorDTO Doctor, int id, [FromQuery] string especialidadeIds)
        {
            DoutorDTOView DoctorView = await Search(id) ?? throw new Exception($"Doutor para o ID: {id} não foi encontrado no banco!");

            var doctorSearch = mapper.Map<DoutorModel>(DoctorView);

            if (Doctor.Documento != null && Doctor.Documento.Length > 0)
            {
                /**
                 * Ativar em produção
                if (!string.IsNullOrEmpty(doctorSearch.DocumentoNome))
                {
                    var previousFilePath = Path.Combine("Storage/DoutoresDocs", doctorSearch.DocumentoNome);
                    if (File.Exists(previousFilePath))
                    {
                        File.Delete(previousFilePath);
                    }
                }
                **/

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Doctor.Documento.FileName);
                var filePath = Path.Combine("Storage/DoutoresDocs", uniqueFileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Doctor.Documento.CopyToAsync(fileStream);
                }

                doctorSearch.DocumentoNome = uniqueFileName;
            }

            doctorSearch.Nome = Doctor.Nome;
            doctorSearch.Email = Doctor.Email;
            doctorSearch.Cpf = Regex.Replace(Doctor.Cpf, "[^0-9]", "");
            doctorSearch.Telefone = Regex.Replace(Doctor.Telefone, "[^0-9]", "");
            doctorSearch.Endereco = Doctor.Endereco;

            await UpdateDoutorEspecialidades(doctorSearch, especialidadeIds);
            dbContext.Doutores.Update(doctorSearch);
            await dbContext.SaveChangesAsync();

            return doctorSearch;
        }

        public async Task<bool> Destroy(int id)
        {
            DoutorDTOView DoctorView = await Search(id) ?? throw new Exception($"Doutor para o ID: {id} não foi encontrado no banco!");

            var doctorSearch = mapper.Map<DoutorModel>(DoctorView);

            dbContext.Doutores.Remove(doctorSearch);
            await dbContext.SaveChangesAsync();
            return true;
        }

        private async Task UpdateDoutorEspecialidades(DoutorModel doctor, string especialidadeIds)
        {
            var especialidadesToRemove = await dbContext.DoutorEspecialidades
                                       .Where(de => de.DoutorId == doctor.Id)
                                       .ToListAsync();

            dbContext.DoutorEspecialidades.RemoveRange(especialidadesToRemove);

            await dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(especialidadeIds))
            {
                List<int> ids = especialidadeIds.Split(',').Select(int.Parse).ToList();

                doctor.DoutorEspecialidades = new List<DoutorEspecialidadeModel>();
               
                foreach (var especialidadeId in ids)
                {
                    var especialidade = await dbContext.Especialidades.FindAsync(especialidadeId);
                    if (especialidade != null)
                    {
                        doctor.DoutorEspecialidades.Add(new DoutorEspecialidadeModel
                        {
                            DoutorId = doctor.Id,
                            EspecialidadeId = especialidade.Id,
                            Doutor = doctor,
                            Especialidade = especialidade
                        });
                    }
                    else
                    {
                        throw new ArgumentException($"Especialidade com ID {especialidadeId} não encontrada.");
                    }
                }
            }
        }

        public async Task<List<EspecialidadeDTO>> EspecialidadesDoutor(string token)
        {
            int doutorId = ExtrairDoutorIdDoToken(token);

            var especialidadesDoutor = dbContext.DoutorEspecialidades
                .Where(de => de.DoutorId == doutorId)
                .Select(de => de.Especialidade)
                .ToList();

            var especialidadesDTO = especialidadesDoutor.Select(e => new EspecialidadeDTO
            {
                Id = e.Id,
                Nome = e.Nome
            }).ToList();

            return especialidadesDTO;
        }

        public async Task<PagedResult<TratamentoDTO>> TratamentosDoutor(string token, string? filterNome, int? filterEspecialidade, int pageNumber, int pageSize)
        {
            int doutorId = ExtrairDoutorIdDoToken(token);

            var especialidadesDoutor = dbContext.DoutorEspecialidades
                .Where(de => de.DoutorId == doutorId)
                .Select(de => de.EspecialidadeId)
                .ToList();

            var query = dbContext.Tratamentos
                .Where(t => especialidadesDoutor.Contains(t.EspecialidadeId))
                .Select(t => new TratamentoDTO
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Tempo = t.Tempo,
                    NomeEspecialidade = t.Especialidade.Nome,
                    EspecialidadeId = t.EspecialidadeId
                });

            if (!string.IsNullOrEmpty(filterNome))
            {
                query = query.Where(t => t.Nome.Contains(filterNome));
            }

            if (filterEspecialidade.HasValue)
            {
                query = query.Where(t => t.EspecialidadeId == filterEspecialidade);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedTratamentos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<TratamentoDTO>
            {
                TotalPages = totalPages,
                Items = paginatedTratamentos
            };

            return result;
        }

        private int ExtrairDoutorIdDoToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);

            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new Exception("Falha ao extrair o ID do doutor do token.");
        }

        public async Task<FileStream> GetImagem(string nomeArquivo)
        {
            var filePath = Path.Combine("Storage/DoutoresDocs", nomeArquivo);

            if (!File.Exists(filePath))
            {
                return null;
            }

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public async Task<DoutorModel> GetByEmail(string email)
        {
            return await dbContext.Doutores
                   .Include(de => de.DoutorEspecialidades)
                   .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
