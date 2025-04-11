using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineDictionary.Utility
{
    public class GoogleTranslateService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GoogleTranslateService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["GoogleApiKey"];
            _httpClient = httpClient;
        }

        public async Task<string> Translate(string text, string targetLanguage)
        {
            var url = $"https://translation.googleapis.com/language/translate/v2?key={_apiKey}";

            var requestData = new
            {
                q = text,
                target = targetLanguage
            };

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
                    var translatedText = jsonResponse
                        .GetProperty("data")
                        .GetProperty("translations")[0]
                        .GetProperty("translatedText")
                        .GetString();

                    return translatedText;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Google Translate API error: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in translation request: {ex.Message}");
            }
        }
    }
}

