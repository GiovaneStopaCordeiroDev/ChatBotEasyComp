using Microsoft.AspNetCore.Mvc;
using ChatBotEasyComp.Services;

namespace ChatBotEasyComp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly OpenAIService _openAIService;

        public ChatController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] string pergunta)
        {
            var resposta = await _openAIService.GetResponse(pergunta);

            return Ok(resposta);
        }
    }
}