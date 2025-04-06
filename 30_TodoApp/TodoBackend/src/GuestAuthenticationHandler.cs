using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

/// <summary>
/// Setzt den angemeldeten User auf guest, wenn keine OpenID Konfiguration eingegeben wurde.
/// Nur für Demonstrationszwecke bei den Übungen vor Authentication.
/// </summary>
public class GuestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public GuestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, "guest"),
            new Claim(ClaimTypes.Role, "Guest")
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
