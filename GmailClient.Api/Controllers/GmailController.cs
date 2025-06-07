using GmailClient.Application.Features.Gmails.Commands.SendEmail;
using GmailClient.Application.Features.Gmails.Queries.GetMessagesList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmailController(IMediator mediator) : Controller
    {
        [HttpGet(Name = "GetAllMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetMessagesListVm>>> GetAllMessages(string accessToken)
        {
            var dtos = await mediator.Send(new GetMessagesListQuery() { accessToken = accessToken });
            return Ok(dtos);
        }

        [HttpPost(Name = "SendEmail")]
        public async Task<ActionResult> SenadEmail(string accessToken, string to, string subject, string body)
        {
            var dtos = await mediator.Send(new SendEmailCommand() { AccessToken = accessToken, Body = body, Subject = subject, To = to });
            return Ok(dtos);
        }

    }
}
