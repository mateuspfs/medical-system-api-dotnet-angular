using Microsoft.AspNetCore.Mvc;
using SistemaMedico.Models;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services
{
    public class AdminService(IAdminRepository adminRepository) : IAdminService
    {

        public async Task<PagedResult<AdminModel>> All(int pageNumber, int pageSize)
        {
            return await adminRepository.All(pageNumber, pageSize);
        }

        public async Task<PagedResult<AdminModel>> Filter(string search, int pageNumber, int pageSize)
        {
            return await adminRepository.Filter(search, pageNumber, pageSize);
        }

        public async Task<AdminModel> Search(int id)
        {
            return await adminRepository.Search(id);
        }

        public async Task<AdminModel> Add(AdminModel admin)
        {
            return await adminRepository.Add(admin);
        }

        public async Task<AdminModel> Att(AdminModel admin, int id)
        {
            return await adminRepository.Att(admin, id);
        }

        public async Task<bool> Destroy(int id)
        {
            return await adminRepository.Destroy(id);
        }
    }
}

