using AzureAdDemo.Extenstions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureAuthentication();
//builder.ConfigureOpenIdAuthentication();

builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevServer",
            policy => policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
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
