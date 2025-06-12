using GmailClient.Application.Contracts;
using GmailClient.Application.DTOs;
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
        public async Task<ActionResult<List<GetMessagesListDto>>> GetAllMessages(string nextPageToken = null)
        {
            var userId = currentUserService.UserId;
            var dtos = await mediator.Send(new GetMessagesListQuery() { UserId = userId, NextPageToken = nextPageToken });
            return Ok(dtos);
        }

        [HttpPost(Name = "SendEmail")]
        public async Task<ActionResult> SendEmail(SendEmailRequest sendEmailRequest)
        {
            var userId = currentUserService.UserId;
            var dtos = await mediator.Send(new SendEmailCommand() { UserId = userId, Body = sendEmailRequest.Body, Subject = sendEmailRequest.Subject, To = sendEmailRequest.To });
            return Ok(dtos);
        }

    }
}
