using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<PagedResult<PacienteDTO>> Filter(string? search, int pageNumber, int pageSize);
        Task<PagedResult<PacienteModel>> All(int pageNumber, int pageSize);
        Task<PacienteModel> Search(int id);
        Task<PacienteModel> Add(PacienteModel paciente);
        Task<PacienteModel> Att(PacienteModel paciente, int id);
        Task<bool> Destroy(int id);
    }
}

