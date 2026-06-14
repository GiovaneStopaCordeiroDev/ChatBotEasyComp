using System.Text;
using System.Text.Json;

namespace ChatBotEasyComp.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpCliente;
        private readonly string _apiKey;
        private readonly string _systemPrompt;

        public OpenAIService(HttpClient httpCliente, IConfiguration config)
        {
            // Ajustado para bater exatamente com o parâmetro recebido
            _httpCliente = httpCliente;
            _apiKey = config["OpenAI:ApiKey"] ?? "";
            _systemPrompt = config["OpenAI:SystemPrompt"] ?? "";
        }

        public async Task<string> GetResponse(string pergunta)
        {
            // Corrigido: Agora declarando os tipos das propriedades no objeto anônimo
            var mensagem = new[]
            {
                new { role = "system", content = _systemPrompt },
                new { role = "user", content = pergunta }
            };

            var corpo = new
            {
                model = "gpt-4o-mini",
                max_tokens = 500,
                messages = mensagem
            };

            var json = JsonSerializer.Serialize(corpo);

            var requisicao = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            requisicao.Headers.Add("Authorization", $"Bearer {_apiKey}");
            requisicao.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var resposta = await _httpCliente.SendAsync(requisicao);
            var conteudo = await resposta.Content.ReadAsStringAsync();

            var documento = JsonDocument.Parse(conteudo);
            var texto = documento.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return texto ?? "";
        }
    }
}