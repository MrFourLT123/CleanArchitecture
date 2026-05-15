using CleanArchitecture.Infrastructure.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

// Add minimal health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Initialize database on startup
// if (app.Environment.IsDevelopment())
// {
//     // await app.InitialiseDatabaseAsync();
// }
// else
// {
//     app.UseHsts();
// }
app.UseHsts();

app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.UseFileServer();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseExceptionHandler(options => { });

#if (UseApiOnly)
app.Map("/", () => Results.Redirect("/scalar"));
#endif

app.MapEndpoints(typeof(Program).Assembly);
app.MapHealthChecks("/health");

#if (!UseApiOnly)
app.MapFallbackToFile("index.html");
#endif

app.Run();
