using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzureAdDemo.Extenstions
{
    public static class WebApplicationBuilderExtension
    {
        public static void ConfigureOpenIdAuthentication(this WebApplicationBuilder builder, IConfigurationSection oidcConfig)
        {
            var authenticationBuilder = builder.Services.AddAuthentication(options =>
            {
                // our authentication process will used signed cookies
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // our authentication challenge is openid
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });
            authenticationBuilder
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.Cookie.SameSite = builder.Environment.IsDevelopment()
                        ? SameSiteMode.None
                        : SameSiteMode.Strict;
                 options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                 options.Cookie.IsEssential = true;
                 options.Events = new CookieAuthenticationEvents
                 {
                     OnRedirectToLogin = ctx =>
                     {
                         ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                         return Task.CompletedTask;
                     },
                     OnRedirectToAccessDenied = ctx =>
                     {
                         ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                         return Task.CompletedTask;
                     }
                 };
             })
             .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
             {
                 options.Authority = oidcConfig["Authority"];
                 options.ClientId = oidcConfig["ClientId"];
                 options.ClientSecret = oidcConfig["ClientSecret"];

                 options.Prompt = "select_account";

                 options.Scope.Add("openid");
                 options.Scope.Add("profile");    // Damit Vor- und Zuname abgefragt werden.
                 options.Scope.Add("user.read");  // Consent, dass die App den Usernamen und das Profil lesen kann.

                 options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.ResponseType = OpenIdConnectResponseType.Code;

                 options.SaveTokens = true;
                 // Dadurch holt ASP.NET Core zusätzliche Claims wie Vorname und Zuname vom UserInfo Endpoint ab.
                 options.GetClaimsFromUserInfoEndpoint = true;

                 options.MapInboundClaims = false;  // Wir verwenden OnTokenValidated um die Keys zu setzen.
                 options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                 options.TokenValidationParameters.RoleClaimType = "roles";

                 options.Events = new OpenIdConnectEvents
                 {
                     OnRedirectToIdentityProvider = ctx =>
                     {
                         if (ctx.Request.Path == "/oauth/login") return Task.CompletedTask;
                         ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                         ctx.HandleResponse();
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = ctx =>
                     {
                         var identity = ctx.Principal?.Identity as ClaimsIdentity;
                         if (identity is null) return Task.CompletedTask;

                         // Das AzureAD liefert das Feld preferred_username.
                         // Wenn ein solches Feld vorhanden ist, verwenden wir dieses als Username.
                         var username = identity.FindFirst(c => c.Type == "preferred_username")?.Value;
                         if (username is not null)
                         {
                             var success = identity.TryRemoveClaim(identity.FindFirst("name"));
                             identity.AddClaim(new Claim(ClaimTypes.Name, username));
                             identity.AddClaim(new Claim("name", username));
                         }
                         return Task.CompletedTask;
                     }
                 };

             });
        }
    }
}
