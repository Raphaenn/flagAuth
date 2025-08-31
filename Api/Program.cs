using Api.Extensions;
using Api.Middlewares;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthenticationAlternative();
// builder.Services.AddJwtAuthentication(secretKey);
builder.Services.RegisterServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "FlagUsers";
    config.Title = "FlagUsers v1";
    config.Version = "v1";

    config.AddSecurity("JWT", new OpenApiSecurityScheme()
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira: Bearer {seu-token-jwt}"
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
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync($"An error ocurred, {e.Message}");
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

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// app.UseCors(policy => policy
//     .AllowAnyHeader()
//     .AllowAnyMethod()
//     .WithOrigins("https://seu-front.app"));

app.RegisterEndpointsDefinitions();

app.Run();