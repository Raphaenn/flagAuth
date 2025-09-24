// Infra/Persistence/ServiceCollectionExtensions.cs

using App.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<InfraDbContext>(opt =>
            opt.UseNpgsql(cfg.GetConnectionString("postgres")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<InfraDbContext>());
        // registre repositórios aqui também, se quiser

        return services;
    }
}