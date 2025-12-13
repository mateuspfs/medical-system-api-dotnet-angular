using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class PacienteService(IPacienteRepository pacienteRepository) : IPacienteService
    {

        public async Task<PagedResult<PacienteDTO>> Filter(string? search, int pageNumber, int pageSize)
        {
            return await pacienteRepository.Filter(search, pageNumber, pageSize);
        }

        public async Task<PagedResult<PacienteModel>> All(int pageNumber, int pageSize)
        {
            return await pacienteRepository.All(pageNumber, pageSize);
        }

        public async Task<PacienteModel> Search(int id)
        {
            return await pacienteRepository.Search(id);
        }

        public async Task<PacienteModel> Add(PacienteModel paciente)
        {
            return await pacienteRepository.Add(paciente);
        }

        public async Task<PacienteModel> Att(PacienteModel paciente, int id)
        {
            return await pacienteRepository.Att(paciente, id);
        }

        public async Task<bool> Destroy(int id)
        {
            return await pacienteRepository.Destroy(id);
        }
    }
}

