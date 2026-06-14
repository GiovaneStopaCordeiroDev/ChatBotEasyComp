using Microsoft.AspNetCore.Mvc;

namespace ChatBotEasyComp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        // Token utilizado no handshake de validação do Webhook da Meta (WhatsApp Business API)
        private const string TokenVerificacaoConstante = "ChaveSecretaEasyComp2026";

        // ==========================================
        // 1. VALIDAÇÃO DO WEBHOOK (MÉTODO GET)
        // ==========================================
        [HttpGet]
        public IActionResult VerifyWebhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string token)
        {
            if (mode == "subscribe" && token == TokenVerificacaoConstante)
            {
                // Retorna o challenge puro exigido pela Meta
                return Content(challenge);
            }

            return Forbid();
        }

        // ==========================================
        // 2. RECEBIMENTO DE MENSAGENS (MÉTODO POST)
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromBody] object payload)
        {
            // Mostra no terminal o JSON da mensagem que chegou
            Console.WriteLine("=== NOVA MENSAGEM RECEBIDA DO WHATSAPP ===");
            Console.WriteLine(payload?.ToString());
            Console.WriteLine("=========================================");

            // Responde 200 OK para o WhatsApp sabendo que deu tudo certo
            return Ok();
        }
    }
}
