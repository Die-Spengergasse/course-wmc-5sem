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
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

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
                 // configuring cookie options
                 options.Cookie.Name = "oidc";
                 options.Cookie.SameSite = SameSiteMode.None;
                 options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                 options.Cookie.IsEssential = true;
             })
             .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
             {
                 var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");

                 options.Authority = oidcConfig["Authority"];
                 options.ClientId = oidcConfig["ClientId"];
                 options.ClientSecret = oidcConfig["ClientSecret"];

                 options.Prompt = "select_account";

                 options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.ResponseType = OpenIdConnectResponseType.Code;

                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;

                 options.MapInboundClaims = false;
                 options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                 options.TokenValidationParameters.RoleClaimType = "roles";
             });
        }
    }
}
