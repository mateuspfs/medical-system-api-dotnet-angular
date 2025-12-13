using Microsoft.AspNetCore.Mvc;
using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class TratamentoPacienteService(ITratamentoPacienteRepository tratamentoPacienteRepository) : ITratamentoPacienteService
    {

        public async Task<PagedResult<TratamentoPacienteListDTO>> DoctorPacientes(string token, int pageNumber, int pageSize, string? search, string? filterTratamento)
        {
            return await tratamentoPacienteRepository.DoctorPacientes(token, pageNumber, pageSize, search, filterTratamento);
        }

        public async Task<TratamentoPacienteModel> Add(string token, TratamentoPacienteAddDTO tp)
        {
            return await tratamentoPacienteRepository.Add(token, tp);
        }

        public async Task<TratamentoPacienteModel> Att(List<IFormFile> arquivos, int id, string token, int? novaEtapaId)
        {
            return await tratamentoPacienteRepository.Att(arquivos, id, token, novaEtapaId);
        }

        public async Task<bool> Destroy(int id)
        {
            return await tratamentoPacienteRepository.Destroy(id);
        }

        public async Task<TratamentoPacienteViewDTO> GetTratamentoPaciente(string token, int tratamentoPacienteId)
        {
            return await tratamentoPacienteRepository.GetTratamentoPaciente(token, tratamentoPacienteId);
        }

        public async Task<FileStream?> GetArquivo(string nomeArquivo)
        {
            return await tratamentoPacienteRepository.GetArquivo(nomeArquivo);
        }
    }
}

