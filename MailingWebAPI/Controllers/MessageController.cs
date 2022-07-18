using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace MailingWebAPI.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private Dictionary<string, int> _db;

        private readonly ILogger<MessageController> _logger;

        public MessageController(Dictionary<string, int> db, ILogger<MessageController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendMessage(MessageToEmails messageToEmails)
        {
            EmailAddressAttribute emailValidator = new EmailAddressAttribute();
            foreach (var email in messageToEmails.Emails)
            {
                //  Проверка корректности email адреса
                if (!emailValidator.IsValid(email))
                {
                    ModelState.AddModelError("error", $"{email} is not a valid e-mail address.");
                    return BadRequest(ModelState);
                }

                // Обновление статистики
                int count;
                bool isSuccess = _db.TryGetValue(email, out count);
                if (isSuccess)
                {
                    _db[email] = ++count;
                }
                else
                {
                    _db.Add(email, 1);
                }
            }

            return Ok();
        }


        [HttpGet("stat")]
        public async Task<ActionResult> GetStat()
        {
            return Ok(_db.Select(kvp => new EmailStat { Email = kvp.Key, MessageCount = kvp.Value }).ToList());
        }


        [HttpGet("count/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMessageCount([EmailAddress] string email)
        {
            int count;
            bool isSuccess = _db.TryGetValue(email, out count);
            if (isSuccess)
            {
                return Ok(count);
            }
            else
            {
                return NotFound();
            }
        }
    }
}