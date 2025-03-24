using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdDemo.Services
{
    /// <summary>
    /// Represents a client for interacting with Azure Active Directory for authentication and token management.
    /// </summary>
    public class AzureAdClient
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;

        /// <summary>
        /// Initializes a new instance of the AzureAdClient class with the provided configuration.
        /// </summary>
        /// <param name="tenantId">The Azure Active Directory tenant ID.</param>
        /// <param name="clientId">The Azure AD application's client ID.</param>
        /// <param name="clientSecret">The client secret for the application.</param>
        /// <param name="scope">The scope of access required for the application.</param>
        public AzureAdClient(string tenantId, string clientId, string clientSecret, string scope)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        /// <summary>
        /// Generates an Azure AD login URL for initiating the OAuth 2.0 authorization code flow.
        /// </summary>
        /// <param name="redirectUrl">The URL to which the user should be redirected after login.</param>
        /// <returns>The Azure AD login URL with the specified parameters.</returns>
        public string GetLoginUrl(string redirectUrl)
        {
            return
                $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/authorize?client_id={_clientId}&response_type=code&redirect_uri={redirectUrl}&response_mode=query&scope=user.read offline_access";
        }

        /// <summary>
        /// Retrieves an access token and optional refresh token using an authorization code.
        /// </summary>
        /// <param name="code">The authorization code obtained after successful user login.</param>
        /// <param name="redirectUrl">The URL to which the user is redirected after authorization.</param>
        /// <returns>A tuple containing the access token and an optional refresh token.</returns>
        public async Task<(string authToken, string? refreshToken)> GetToken(string code, string redirectUrl)
        {
            string formdata =
                $"client_id={_clientId}&scope={_scope}&code={code}&redirect_uri={redirectUrl}&grant_type=authorization_code&client_secret={_clientSecret}";
            return await RequestTokens(formdata);
        }

        /// <summary>
        /// Creates a Microsoft Graph Service Client instance using the provided access token.
        /// </summary>
        /// <param name="token">The access token to use for Microsoft Graph API requests.</param>
        /// <returns>A GraphServiceClient instance authenticated with the access token.</returns>
        public static GraphServiceClient GetGraphServiceClientFromToken(string token)
        {
            var authProvider = new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                return Task.CompletedTask;
            });

            return new GraphServiceClient(authProvider);
        }

        /// <summary>
        /// Sends a request to Azure AD to obtain access and refresh tokens.
        /// </summary>
        /// <param name="formdata">The form data for the token request.</param>
        /// <returns>A tuple containing the access token and an optional refresh token.</returns>
        private async Task<(string authToken, string? refreshToken)> RequestTokens(string formdata)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/");
            var response = await client.PostAsync("token",
                new StringContent(formdata, Encoding.UTF8, "application/x-www-form-urlencoded"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var data = System.Text.Json.JsonDocument.Parse(content).RootElement;
            var authToken = data.GetProperty("access_token").GetString() ??
                            throw new ApplicationException("Missing auth token in response.");
            var refreshToken = data.TryGetProperty("refresh_token", out var val) ? val.GetString() : null;
            return (authToken, refreshToken);
        }

    }
}