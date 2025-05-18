using AzureAdDemo.Extenstions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TodoBackend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Im Kapitel OAuth konfigurieren wir das Paket OpenIDConnect, das die Authentifizierung mit OAuth 2.0 und OpenID Connect ermöglicht.
// Ist kein Eintrag für das App Service im Azure AD konfiguriert (für die Übungen vor Authentication), wird immer guest als Benutzername verwendet
// und ein gültiges Login angenommen.
var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");
if (oidcConfig.Exists())
{
    builder.ConfigureOpenIdAuthentication(oidcConfig);
}
else 
{
    Console.WriteLine($"[INFO] No OpenIDConnectSettings provided in appsettings.json. We use guest as username.");
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddAuthentication("GuestScheme")
               .AddScheme<AuthenticationSchemeOptions, GuestAuthenticationHandler>("GuestScheme", null);
    }
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DrivingExamContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

// Set CORS mode for vue devserver.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowDevserver",
            builder => builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
    });
}

var app = builder.Build();

// Creating the database.
using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<DrivingExamContext>())
    {
        db.Initialize(deleteDatabase: app.Environment.IsDevelopment());
        db.Seed();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowDevserver");
}

// app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// app.UseStaticFiles();

app.Start();
var color = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("+-------------+");
Console.WriteLine("| API started |");
Console.WriteLine("+-------------+");
Console.WriteLine($"Visit swagger running on {app.Urls.First(u => u.StartsWith("https"))}/swagger\n");
Console.ForegroundColor = color;
app.WaitForShutdown();

