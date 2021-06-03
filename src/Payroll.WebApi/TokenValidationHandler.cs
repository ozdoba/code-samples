using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Payroll.WebApi
{
    public static class AuthenticationSchemeConstants
    {
        public const string TokenValidationScheme = "ValidateToken";
    }
    
    public class TokenValidationSchemeOptions : AuthenticationSchemeOptions
    {
    }

    public class TokenValidationHandler : AuthenticationHandler<TokenValidationSchemeOptions>
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        
        public TokenValidationHandler(
            IOptionsMonitor<TokenValidationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            HttpClient client) 
        : base(options, logger, encoder, clock)
        {
            _client = client;
            _logger = logger.CreateLogger<TokenValidationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return AuthenticateResult.Fail("Header Not Found.");
            }

            var authHeaderValue = Request.Headers[HeaderNames.Authorization];
            var authHeader = AuthenticationHeaderValue.Parse(authHeaderValue);

            HttpResponseMessage validationResponse = null;
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://devsso2.marcura.com:8080/openam/oauth2/tokeninfo"),
                    Method = HttpMethod.Get
                };
                request.Headers.Authorization = authHeader;

                validationResponse = await _client.SendAsync(request);

                if (!validationResponse.IsSuccessStatusCode)
                {
                    return AuthenticateResult.Fail(
                        "Unable to validate token. Unsuccessful response from tokeninfo endpoint");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Unable to send token validation request.");
            }

            TokenInformation tokenInfo = null;
            try
            {
                tokenInfo =
                    JsonSerializer.Deserialize<TokenInformation>(await validationResponse.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Unable to deserialize TokenInformation");
            }

            if (null == tokenInfo)
            {
                return AuthenticateResult.Fail("TokenInformation is null");
            }
                
            if (!string.IsNullOrEmpty(tokenInfo.Error))
            {
                return AuthenticateResult.Fail(tokenInfo.ErrorDescription);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, tokenInfo.ClientId),
            };
            claims.AddRange(tokenInfo.Scopes.Select(s => new Claim(ClaimTypes.Role, s)));
            
            var claimsIdentity = new ClaimsIdentity(claims,
                nameof(TokenValidationHandler));

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    internal class TokenInformation
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
        [JsonPropertyName("scope")]
        public IEnumerable<string> Scopes { get; set; }
        [JsonPropertyName("client_id")]
        public string ClientId { get; set;}
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}