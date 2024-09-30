using DataverseAPI.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;


namespace DataverseAPI.Helpers
{
    public class EmbarquesHelper
    {
        private readonly AzureAdOptions _azureAdOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmbarquesHelper(IOptions<AzureAdOptions> azureAdOptions, IHttpClientFactory httpClientFactory) {

            _azureAdOptions = azureAdOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> PostDataverseDataAsync(string entity, object data)
        {
            try
            {
                //var client = _httpClientFactory.CreateClient();
                string dataverseUrl = _azureAdOptions.DataverseUrl;
                var accessToken = await GetAccessToken();

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await client.PostAsync($"{dataverseUrl}{entity}", content);

                //if (!response.IsSuccessStatusCode)
                //{
                //    var errorResponse = await response.Content.ReadAsStringAsync();
                //    throw new Exception($"Error posting data to Dataverse: {response.StatusCode}, Details: {errorResponse}");
                //}

                //string result = await response.Content.ReadAsStringAsync();
                //return result;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://neutralqa.crm.dynamics.com/api/data/v9.1/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync($"{dataverseUrl}{entity}", content);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error posting data to Dataverse: {response.StatusCode}, Details: {errorResponse}");
                    }

                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while posting data to Dataverse: {ex.Message}, StackTrace: {ex.StackTrace}", ex);
            }
        }

        private async Task<string> GetAccessToken()
        {
            string tenantId = _azureAdOptions.TenantId;
            string clientId = _azureAdOptions.ClientId;
            string clientSecret = _azureAdOptions.ClientSecret;
            string scopeUrl = _azureAdOptions.Scope;
            string loginMicrosoftURL = _azureAdOptions.Instance;

            string authority = $"{loginMicrosoftURL}/{tenantId}";

            var clientApp = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();


            var scopes = new[] { scopeUrl }; // Replace with your Dataverse URL

            var result = await clientApp.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;

            //    var authContextUrl = $"{loginMicrosoftURL}{tenantId}/oauth2/v2.0/token";
            //    var requestBody = new FormUrlEncodedContent(new[]
            //    {
            //        new KeyValuePair<string, string>("client_id", clientId),
            //        new KeyValuePair<string, string>("client_secret", clientSecret),
            //        new KeyValuePair<string, string>("grant_type", "client_credentials"),
            //        new KeyValuePair<string, string>("scope", scopeUrl)
            //    });

            //    try
            //    {
            //        var client = _httpClientFactory.CreateClient();
            //        HttpResponseMessage response = await client.PostAsync(authContextUrl, requestBody);
            //        response.EnsureSuccessStatusCode();

            //        var responseBody = await response.Content.ReadAsStringAsync();
            //        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            //        if (tokenResponse.TryGetProperty("access_token", out JsonElement accessTokenElement))
            //        {
            //            return accessTokenElement.GetString();
            //        }
            //        throw new Exception("Access token not found in the response.");
            //    }
            //    catch (HttpRequestException httpRequestException)
            //    {
            //        throw new Exception($"Network error while trying to get access token: {httpRequestException.Message}", httpRequestException);
            //    }
            //    catch (JsonException jsonException)
            //    {
            //        throw new Exception($"Error parsing response while getting access token: {jsonException.Message}", jsonException);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception($"An error occurred while getting access token: {ex.Message}", ex);
            //    }
            }
        }
}
