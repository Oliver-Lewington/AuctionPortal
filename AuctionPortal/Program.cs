using AuctionPortal.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database connection
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(conn));

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

builder.Services.AddMudServices(); // Adds MudBlazor styles and services

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// --- Data Protection Setup ---
// Path to persist keys (mounted volume)
var keysFolder = "/app/keys";
Directory.CreateDirectory(keysFolder);

var dataProtectionBuilder = builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("AuctionPortal");

// Encrypt keys with certificate if available
var certPath = "/app/certs/dataprotection.pfx";
var certPassword = "certPassword"; // replace with your real password or secret (pass in from env var or secret manager)

if (File.Exists(certPath))
{
    var cert = new X509Certificate2(certPath, certPassword);
    dataProtectionBuilder.ProtectKeysWithCertificate(cert);
}

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
    db.Database.Migrate();
}

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAntiforgery();
}

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();
app.Run();