using CleanArchitecture.Infrastructure.Data;
using Scalar.AspNetCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

// Add minimal health checks
builder.Services.AddHealthChecks();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["X-API-KEY"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = "X-API-KEY" // must match APIKEYNAME exactly
        };

        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("X-API-KEY", document)] = new List<string>()
        });

        return Task.CompletedTask;
    });
});
var app = builder.Build();

// Initialize database on startup
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.UseFileServer();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .AddPreferredSecuritySchemes("X-API-KEY")
        .AddApiKeyAuthentication("X-API-KEY", scheme =>
        {
            scheme.Value = "CodeX2026!@#";
        });
});
app.UseExceptionHandler(options => { });

// Enforce App-level security first
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

#if (UseApiOnly)
app.Map("/", () => Results.Redirect("/scalar"));
#endif

app.MapEndpoints(typeof(Program).Assembly);
app.MapHealthChecks("/health");

#if (!UseApiOnly)
app.MapFallbackToFile("index.html");
#endif

app.Run();
