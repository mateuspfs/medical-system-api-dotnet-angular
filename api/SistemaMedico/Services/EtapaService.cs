using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;

namespace SistemaMedico.Services
{
    public class EtapaService(IEtapaRepository etapaRepository) : IEtapaService
    {

        public async Task<List<EtapaModel>> All()
        {
            return await etapaRepository.All();
        }

        public async Task<EtapaModel> Search(int id)
        {
            return await etapaRepository.Search(id);
        }

        public async Task<EtapaPacienteDTO> PacientesEtapa(string? search, int pageNumber, int pageSize, int id)
        {
            return await etapaRepository.PacientesEtapa(search, pageNumber, pageSize, id);
        }

        public async Task<EtapaTratamentoDTO> SearchTratamento(int id)
        {
            return await etapaRepository.SearchTratamento(id);
        }

        public async Task<EtapaModel> Add(EtapaModel etapa)
        {
            return await etapaRepository.Add(etapa);
        }

        public async Task<EtapaModel> Att(EtapaModel etapa, int id)
        {
            return await etapaRepository.Att(etapa, id);
        }

        public async Task<bool> Destroy(int id)
        {
            return await etapaRepository.Destroy(id);
        }
    }
}

