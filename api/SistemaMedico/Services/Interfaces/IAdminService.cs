using Microsoft.AspNetCore.Mvc;
using SistemaMedico.Models;
using SistemaMedico.Utilies;

namespace SistemaMedico.Services.Interfaces
{
    public interface IAdminService
    {
        Task<PagedResult<AdminModel>> All(int pageNumber, int pageSize);
        Task<PagedResult<AdminModel>> Filter(string search, int pageNumber, int pageSize);
        Task<AdminModel> Search(int id);
        Task<AdminModel> Add(AdminModel admin);
        Task<AdminModel> Att(AdminModel admin, int id);
        Task<bool> Destroy(int id);
    }
}

