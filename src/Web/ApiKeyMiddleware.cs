using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-API-Key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Let Scalar UI and the OpenAPI document through without an API key
        if (path.StartsWith("/scalar", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/openapi", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // 1. Check if the header exists
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Application API Key is missing.");
            return;
        }

        // 2. Fetch the valid key from appsettings.json
        string? apiKey = configuration.GetValue<string>("Authentication:AppApiKey");

        // 3. Validate the key
        if (string.IsNullOrEmpty(apiKey) || apiKey != extractedApiKey.ToString())
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized application." + " Please provide a valid API key. Current key: " + extractedApiKey.ToString() + ", Valid key: " + apiKey);
            return;
        }

        // Key is valid, proceed to the next middleware (User Auth)
        await _next(context);
    }
}
