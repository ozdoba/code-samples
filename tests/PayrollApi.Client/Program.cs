using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayrollApi.Client
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        
        static async Task Main(string[] args)
        {
            await RequestToken();
            await GetHEmployees();
        }

        private static async Task GetHEmployees()
        {
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:9100/payroll/employees/v1"),
                Method = HttpMethod.Get
            };
            var msg = await client.SendAsync(message);
            
            Console.WriteLine(message.Headers.Authorization.ToString());

            msg.EnsureSuccessStatusCode();
            Console.WriteLine(await msg.Content.ReadAsStringAsync());
        }
        
        private static async Task RequestToken()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            var data = new Dictionary<string, string>()
            {
                {"grant_type", "client_credentials"},
                {"client_id","MartrustOAuthClient"},
                {"client_secret","clientP@ssword"},
                {"scope","payroll:read payroll:write employees:read"},
            };

            var message = new HttpRequestMessage
            {
                RequestUri =
                    new Uri("http://devsso2.marcura.com:8080/openam/oauth2/realms/root/realms/marcura/access_token"),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(data)
            };
            

            var msg = await client.SendAsync(message);
            
            msg.EnsureSuccessStatusCode();

            var response = JsonSerializer.Deserialize<AuthResopnse>(await msg.Content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(response.TokenType, response.AccessToken);
            Console.WriteLine(client.DefaultRequestHeaders.Authorization);
        }
        
        
    }

    public class AuthResopnse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } // c1d6e3b5-f5b1-4de7-8f10-ab8a4426f37d
        [JsonPropertyName("scope")]
        public string Scope { get; set; } //"payroll:write payroll:read",
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }// Bearer,
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } //3599
    }
}