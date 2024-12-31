using Api.Abstractions;

namespace Api.Extensions;

public static class EndpointsExtensions
{
    public static void RegisterEndpointsDefinitions(this WebApplication application)
    {
        IEnumerable<IEndpointsDefinitions> endpointsDefinitions = typeof(Program).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IEndpointsDefinitions)) && !t.IsAbstract && !t.IsInterface).Select(Activator.CreateInstance).Cast<IEndpointsDefinitions>();

        foreach (var endpoints in endpointsDefinitions)
        {
            endpoints.RegisterEndpoints(application);
        }
    }
}