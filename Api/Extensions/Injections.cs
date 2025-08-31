using App.Dto;
using App.IRepository;
using App.Services;
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
        serviceCollection.AddScoped<IFriendRepository, FriendRepository>();
        serviceCollection.AddScoped<IUserPhotoRepository, UserPhotosRepository>();
        serviceCollection.AddScoped<IPreferencesRepository, PreferencesRepository>();
        serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        serviceCollection.AddScoped<ITokenService, TokenService>();


        
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));


        return serviceCollection;
    }
}