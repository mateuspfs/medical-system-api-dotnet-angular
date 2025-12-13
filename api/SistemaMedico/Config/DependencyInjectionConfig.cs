using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaMedico.Data;
using SistemaMedico.Repositories;
using SistemaMedico.Repositories.Interfaces;
using SistemaMedico.Services;
using SistemaMedico.Services.Interfaces;
using SistemaMedico.Mappings;
using SistemaMedico.Utilies;

namespace SistemaMedico.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            // Database Context
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<SistemaMedicoDBContex>(
                    options => options.UseSqlServer(configuration.GetConnectionString("DataBase"))
                );

            // Repositories
            services.AddScoped<IDoutorRepository, DoutorRepository>();
            services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<ITratamentoRepository, TratamentoRepository>();
            services.AddScoped<IEtapaRepository, EtapaRepository>();
            services.AddScoped<ITratamentoPacienteRepository, TratamentoPacienteRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();

            // Services
            services.AddScoped<IDoutorService, DoutorService>();
            services.AddScoped<IEspecialidadeService, EspecialidadeService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IEtapaService, EtapaService>();
            services.AddScoped<ITratamentoService, TratamentoService>();
            services.AddScoped<ITratamentoPacienteService, TratamentoPacienteService>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<EmailService>();

            // AutoMapper
            services.AddAutoMapper(typeof(EnitiesDTO));

            return services;
        }
    }
}

