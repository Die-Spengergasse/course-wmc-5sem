using AzureAdDemo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzureAdDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;     // Kann Einstellungen aus appsettings.json lesen.
        private readonly AzureAdClient _adClient;
        private string LoginRedirectUrl => $"https://{HttpContext.Request.Host}/users/authorize";

        public UsersController(AzureAdClient adClient, IConfiguration config)
        {
            _adClient = adClient;
            _config = config;
        }

        [HttpGet("login")]
        public IActionResult LoginRedirect()
        {
            return Redirect(_adClient.GetLoginUrl(LoginRedirectUrl));
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogoutRedirect()
        {
            await HttpContext.SignOutAsync();
            return Redirect("https://login.microsoftonline.com/logout.srf");
        }

        [HttpGet("authorize")]
        [Authorize]
        public async Task<IActionResult> Authorize([FromQuery] string code)
        {
            var (authToken, refreshToken) = await _adClient.GetToken(code, LoginRedirectUrl);

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("No refresh token in payload.");

            // Liest über Microsoft Graph zusätzliche Informationen über den User aus.
            var graph = AzureAdClient.GetGraphServiceClientFromToken(authToken);
            var me = await graph.Me.Request().Select("UserPrincipalName,EmployeeId,GivenName,Surname,OfficeLocation")
                .GetAsync();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, me.GivenName),
                new Claim(ClaimTypes.Surname, me.Surname),
                new Claim(ClaimTypes.Name, me.UserPrincipalName),
                new Claim(ClaimTypes.Role, "admin")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
            };

            // Create a Cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // OR create a jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _config.GetRequiredSection("Secrets")["Jwt"];
            if (secret == null) throw new ApplicationException("Missing JWT Secret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Payload for our JWT.
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Convert.FromBase64String(secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                Token = tokenString
            });
        }

        [HttpGet("me")]
        // Wichtig, denn der Controller hat [AllowAnonymous].
        // Sonst ist HttpContext.User.Identity leer.
        [Authorize]
        public IActionResult GetUserDetails()
        {
            var authenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (!authenticated) { return Unauthorized(); }
            var firstname = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastname = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            return Ok(new
            {
                firstname,
                lastname,
                Username = HttpContext.User.Identity?.Name,
                IsAdmin = HttpContext.User.IsInRole("admin"),
            });
        }
    }
}
