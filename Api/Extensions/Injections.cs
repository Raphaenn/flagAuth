using App.IRepository;
using App.Users.Queries;
using Infra;
using Infra.Repository;

namespace Api.Extensions;


public static class Injections
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<InfraDbContext>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IAuthRepository, AuthRepository>();
        
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));


        return serviceCollection;
    }
}