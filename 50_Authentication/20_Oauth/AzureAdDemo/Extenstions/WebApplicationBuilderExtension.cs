using AzureAdDemo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzureAdDemo.Extenstions
{
    public static class WebApplicationBuilderExtension
    {
        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            // Azure AD Client
            builder.Services.AddScoped(provider => new AzureAdClient(
                tenantId: builder.Configuration["AzureAd:TenantId"] ?? throw new ArgumentException("AzureAd:TenantId is null"),
                clientId: builder.Configuration["AzureAd:ClientId"] ?? throw new ArgumentException("AzureAd:ClientId is null"),
                clientSecret: builder.Configuration["AzureAd:ClientSecret"] ??
                              throw new ArgumentException("AzureAd:ClientSecret is null"),
                scope: builder.Configuration["AzureAd:Scope"] ?? throw new ArgumentException("AzureAd:Scope is null")));

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.OnAppendCookie = cookieContext =>
                {
                    cookieContext.CookieOptions.Secure = true;
                    cookieContext.CookieOptions.SameSite = builder.Environment.IsDevelopment()
                        ? SameSiteMode.None
                        : SameSiteMode.Strict;
                };
            });
            // JWT Authentication ******************************************************************************
            // using Microsoft.AspNetCore.Authentication.JwtBearer;
            // using Microsoft.IdentityModel.Tokens;

            var jwtSecret = builder.Configuration.GetRequiredSection("Secrets")["Jwt"];
            if (jwtSecret == null) throw new ApplicationException("Missing JWT Secret.");
            byte[] secret = Convert.FromBase64String(jwtSecret);
            builder.Services
                .AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/users/login";
                    options.AccessDeniedPath = "/users/logout";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });
            var multiSchemePolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser()
              .Build();

            builder.Services.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy);
        }
        public static void ConfigureOpenIdAuthentication(this WebApplicationBuilder builder)
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
                 var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");

                 options.Authority = oidcConfig["Authority"];
                 options.ClientId = oidcConfig["ClientId"];
                 options.ClientSecret = oidcConfig["ClientSecret"];

                 options.Prompt = "select_account";

                 options.Scope.Add("openid");
                 options.Scope.Add("profile");  // Damit Vor- und Zuname abgefragt werden.

                 options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.ResponseType = OpenIdConnectResponseType.Code;

                 options.SaveTokens = true;
                 // Dadurch holt ASP.NET Core zusätzliche Claims wie Vorname und Zuname vom UserInfo Endpoint ab.
                 options.GetClaimsFromUserInfoEndpoint = true;

                 options.MapInboundClaims = false;  // Wir verwenden OnTokenValidated um die Keye zu setzen.
                 options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                 options.TokenValidationParameters.RoleClaimType = "roles";

                 // Um genauere Inforationen über den angemeldeten User herauszufinden, fragen wir
                 // über den Microsoft Graph Client das AD ab.
                 // Das brauchen wir nur bei einer Azure AD Authentifizierung. Bei anderen Providern ist es vielleicht anders.
                 options.Events = new OpenIdConnectEvents
                 {
                     OnRedirectToIdentityProvider = ctx =>
                     {
                         if (ctx.Request.Path == "/oauth/login") return Task.CompletedTask;
                         ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                         ctx.HandleResponse();
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = async ctx =>
                     {
                         var identity = ctx.Principal?.Identity as ClaimsIdentity;
                         if (identity is null) return;

                         var token = ctx.TokenEndpointResponse?.AccessToken;
                         if (!string.IsNullOrEmpty(token))
                         {
                             var authProvider = new DelegateAuthenticationProvider(request =>
                             {
                                 request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                                 return Task.CompletedTask;
                             });
                             var graphClient = new GraphServiceClient(authProvider);
                             var me = await graphClient.Me
                                .Request()
                                .Select("UserPrincipalName,EmployeeId,GivenName,Surname,OfficeLocation")
                                .GetAsync();

                             if (!string.IsNullOrEmpty(me.GivenName))
                                 identity.AddClaim(new Claim(ClaimTypes.GivenName, me.GivenName));

                             if (!string.IsNullOrEmpty(me.Surname))
                                 identity.AddClaim(new Claim(ClaimTypes.Surname, me.Surname));
                         }
                     }
                 };

             });
        }
    }
}
