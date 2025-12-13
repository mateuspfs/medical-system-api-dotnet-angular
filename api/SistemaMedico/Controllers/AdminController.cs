using Microsoft.AspNetCore.Mvc;
using SistemaMedico.Helpers;
using SistemaMedico.Models;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Utilies;

namespace SistemaMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AdminModel>>>> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var admins = await adminService.All(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<AdminModel>>.SuccessResponse(admins));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<AdminModel>>.ErrorResponse($"Erro ao buscar admins: {ex.Message}"));
            }
        }
        
        [HttpGet("filter")]
        public async Task<ActionResult<ApiResponse<PagedResult<AdminModel>>>> Filter(string search, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var admins = await adminService.Filter(search, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<AdminModel>>.SuccessResponse(admins));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PagedResult<AdminModel>>.ErrorResponse($"Erro ao filtrar admins: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AdminModel>>> Search(int id)
        {
            try
            {
                var admin = await adminService.Search(id);
                return admin == null
                    ? NotFound(ApiResponse<AdminModel>.ErrorResponse($"Admin com ID {id} não encontrado."))
                    : Ok(ApiResponse<AdminModel>.SuccessResponse(admin));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<AdminModel>.ErrorResponse($"Erro ao buscar admin: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AdminModel>>> Add(AdminModel admin)
        {
            try
            {
                var newAdmin = await adminService.Add(admin);
                return Ok(ApiResponse<AdminModel>.SuccessResponse(newAdmin, "Admin criado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<AdminModel>.ErrorResponse($"Erro ao criar admin: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AdminModel>>> Att(AdminModel admin, int id)
        {
            try
            {
                var adminAtt = await adminService.Att(admin, id);
                return Ok(ApiResponse<AdminModel>.SuccessResponse(adminAtt, "Admin atualizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<AdminModel>.ErrorResponse($"Erro ao atualizar admin: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Destroy(int id)
        {
            try
            {
                bool destroy = await adminService.Destroy(id);
                return destroy
                    ? Ok(ApiResponse<bool>.SuccessResponse(true, "Admin removido com sucesso."))
                    : BadRequest(ApiResponse<bool>.ErrorResponse("Erro ao remover admin."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse($"Erro ao remover admin: {ex.Message}"));
            }
        }
    }
}
