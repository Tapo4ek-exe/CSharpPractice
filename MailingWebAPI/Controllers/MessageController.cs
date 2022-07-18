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


        /// <summary>
        /// Sends message to emails
        /// </summary>
        /// <param name="messageToEmails">MessageToEmails object</param>
        /// <remarks>
        /// Sample request:
        /// POST api/messages/send
        ///{
        ///     "emails": [
        ///         "test1@email.com",
        ///         "test2@email.com"
        ///     ],
        ///     "message": "Hello!"
        ///}
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the messageToEmails is invalid</response>
        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendMessage(MessageToEmails messageToEmails)
        {
            EmailAddressAttribute emailValidator = new EmailAddressAttribute();
            foreach (var email in messageToEmails.Emails)
            {
                _logger.LogInformation($"Processing request {Request.Path}");

                //  Проверка корректности email адреса
                if (!emailValidator.IsValid(email))
                {
                    _logger.LogWarning($"Request {Request.Path}: invalid email");
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
            _logger.LogInformation($"Request {Request.Path} passed");
            return Ok();
        }


        /// <summary>
        /// Gets the stats of each email
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET api/messages/stat
        /// </remarks>
        /// <returns>Stats of each email</returns>
        /// <response code="200">Success</response>
        [HttpGet("stat")]
        public async Task<ActionResult> GetStat()
        {
            _logger.LogInformation($"Processing request {Request.Path}");

            return Ok(_db.Select(kvp => new EmailStat { Email = kvp.Key, MessageCount = kvp.Value }).ToList());
        }


        /// <summary>
        /// Gets the number of messages sent to a given email
        /// </summary>
        /// <param name="email">Email for which the number of messages is counted</param>
        /// <remarks>
        /// Sample request:
        /// GET api/messages/count/test1@email.com
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the email is invalid</response>
        /// <response code="404">If the email not found</response>
        [HttpGet("count/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMessageCount([EmailAddress] string email)
        {
            _logger.LogInformation($"Processing request {Request.Path}");

            int count;
            bool isSuccess = _db.TryGetValue(email, out count);
            if (isSuccess)
            {
                _logger.LogInformation($"Request {Request.Path} passed");
                return Ok(count);
            }
            else
            {
                _logger.LogWarning($"Request {Request.Path}: email not found");
                return NotFound();
            }
        }
    }
}