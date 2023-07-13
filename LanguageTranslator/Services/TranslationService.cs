using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace YourProjectName.Services
{
    public class TranslationService
    {
        private readonly string _openAIKey;

        public TranslationService(IConfiguration configuration)
        {
            _openAIKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> TranslateText(string inputText, string inputLanguage, string outputLanguage)
        {
            var prompt = $"Translate the following text from {inputLanguage} to {outputLanguage}: \"{inputText}\"";
            var requestBody = JsonConvert.SerializeObject(new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = prompt },
            new { role = "user", content = "" }
        }
            });

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAIKey}");

                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Translation failed with status code: {response.StatusCode}. Response content: {responseContent}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var translatedText = ExtractTranslatedText(responseBody);

                return translatedText;
            }
        }



        private static string ExtractTranslatedText(string response)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(response);
            var translatedText = json.choices[0].message.content;

            return translatedText;
        }
    }
}
