using App.Dto;
using App.IRepository;
using App.Services;
using App.Users.Queries;
using Infra;
using Infra.Messaging;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class Injections
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration config)
    {
        serviceCollection.AddDbContext<InfraDbContext>(opt => opt.UseNpgsql(config.GetConnectionString("PostgresRepository")));
        serviceCollection.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<InfraDbContext>());
        
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IAuthRepository, AuthRepository>();
        serviceCollection.AddScoped<IFriendRepository, FriendRepository>();
        serviceCollection.AddScoped<IUserPhotoRepository, UserPhotosRepository>();
        serviceCollection.AddScoped<IPreferencesRepository, PreferencesRepository>();
        serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        serviceCollection.AddScoped<ITokenService, TokenService>();

        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));
        
        serviceCollection.Configure<KafkaOptions>(config.GetSection("kafka"));
        serviceCollection.AddSingleton<IKafkaProducer, KafkaProducer>();
        serviceCollection.AddScoped<IIntegrationEventPublisher, OutboxPublisher>();
        serviceCollection.AddScoped<IUserProjectionWriter, UserProjectionWriter>();
        serviceCollection.AddHostedService<OutboxDispatcher>();  // publica Outbox → Kafka
        serviceCollection.AddHostedService<UserEventsConsumer>(); // consome Kafka → ReadModel


        return serviceCollection;
    }
}