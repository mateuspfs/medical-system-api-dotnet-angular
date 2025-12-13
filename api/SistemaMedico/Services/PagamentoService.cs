using SistemaMedico.DTOs;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class PagamentoService(IPagamentoRepository pagamentoRepository) : IPagamentoService
    {

        public async Task<PagamentoEtapaModel> Search(int id)
        {
            return await pagamentoRepository.Search(id);
        }

        public async Task<PagamentoResponse> Add(string token, PagamentoEtapaDTO pagamentoDTO, int tratamentoId)
        {
            return await pagamentoRepository.Add(token, pagamentoDTO, tratamentoId);
        }

        public async Task<PagamentoEtapaModel> Att(PagamentoEtapaModel pagamento, int id)
        {
            return await pagamentoRepository.Att(pagamento, id);
        }

        public async Task<bool> Destroy(int id)
        {
            return await pagamentoRepository.Destroy(id);
        }
    }
}

