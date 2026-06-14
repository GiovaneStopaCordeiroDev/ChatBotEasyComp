using System.Text;
using System.Text.Json;

namespace ChatBotEasyComp.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _systemPrompt;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"] ?? string.Empty;
            _systemPrompt = configuration["OpenAI:SystemPrompt"] ?? string.Empty;
        }

        public async Task<string> GetResponse(string pergunta)
        {
            var mensagens = new[]
            {
                new
                {
                    role = "system",
                    content = _systemPrompt
                },
                new
                {
                    role = "user",
                    content = pergunta
                }
            };

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = mensagens,
                max_tokens = 500
            };

            var json = JsonSerializer.Serialize(requestBody);

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/chat/completions"
            );

            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Erro ao chamar OpenAI: {response.StatusCode}\n{error}"
                );
            }

            var content = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(content);

            var resposta = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return resposta ?? "Nenhuma resposta encontrada.";
        }
    }
}