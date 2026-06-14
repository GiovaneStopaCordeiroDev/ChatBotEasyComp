using Microsoft.AspNetCore.Mvc;
using ChatBotEasyComp.Services;
using ChatBotEasyComp.Models;

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
        public async Task<IActionResult> Chat([FromBody] ChatRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Pergunta))
            {
                return BadRequest("A pergunta não pode estar vazia.");
            }

            try
            {
                var resposta = await _openAIService.GetResponse(request.Pergunta);

                return Ok(new ChatResponseModel
                {
                    Resposta = resposta
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}