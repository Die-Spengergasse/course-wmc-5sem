using AzureAdDemo.Extenstions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Variante 1: OAuth mit manuellem Service.
// NICHT FÜR PRODUCTION PROJEKTE, nur zur Demonstration für den Flow.
builder.ConfigureAuthentication();
// Variante 2: OAuth mit Microsoft.AspNetCore.Authentication.OpenIdConnect
// Bevorzugte Variante für OAuth Projekte
//builder.ConfigureOpenIdAuthentication();

builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevServer",
            policy => policy.SetIsOriginAllowed(origin => true)
                .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevServer");
    app.UseCookiePolicy();
}

app.UseAuthentication();
// Authorization is applied for middleware after the UseAuthorization method
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
