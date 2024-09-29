using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoBackend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

// For cross origin cookies we need a secure cookie (https only) and samesite None in dev mode.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.OnAppendCookie = cookieContext =>
    {
        cookieContext.CookieOptions.Secure = true;
        cookieContext.CookieOptions.SameSite = builder.Environment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Strict;
    };
});

// Configure cookie. By default ASP sends a redirect to the login page. This makes no sense with a SPA,
// so we send 401 if we try to request a protected route.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return System.Threading.Tasks.Task.CompletedTask;
        };
    });

// Set CORS mode for vue devserver.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDevserver",
        builder => builder.SetIsOriginAllowed(origin => new System.Uri(origin).IsLoopback)
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});


var app = builder.Build();

// Creating the database.
using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<TodoContext>())
    {
        db.Initialize(deleteDatabase: true);
        db.Seed();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowDevserver");
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.Run();
