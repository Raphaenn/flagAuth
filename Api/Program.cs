using Api.Extensions;
using Api.Middlewares;
using NSwag;
using NSwag.Generation.Processors.Security;

string secretKey = "esta-e-uma-chave-secreta-de-32-bits";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication();
builder.Services.RegisterServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "FlagUsers";
    config.Title = "FlagUsers v1";
    config.Version = "v1";

    config.AddSecurity("Bearer", new OpenApiSecurityScheme()
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu-token-jwt}"
        
    });
    
    // Configuração para exigir o token JWT em rotas protegidas
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync("An error ocurred");
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "FlagUsers";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseHttpsRedirection();
app.RegisterEndpointsDefinitions();

app.Run();