using GmailClient.Application.Contracts;
using GmailClient.Application.Features.Gmails.Commands.SendEmail;
using GmailClient.Application.Features.Gmails.Queries.GetMessagesList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmailController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {
        [HttpGet(Name = "GetAllMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetMessagesListVm>>> GetAllMessages()
        {
            var userId = currentUserService.UserId;
            var dtos = await mediator.Send(new GetMessagesListQuery() { UserId = userId });
            return Ok(dtos);
        }

        [HttpPost(Name = "SendEmail")]
        public async Task<ActionResult> SenadEmail(string to, string subject, string body)
        {
            var userId = currentUserService.UserId;
            var dtos = await mediator.Send(new SendEmailCommand() { UserId = userId, Body = body, Subject = subject, To = to });
            return Ok(dtos);
        }

    }
}
