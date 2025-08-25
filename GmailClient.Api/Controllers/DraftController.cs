using GmailClient.Application.Contracts;
using GmailClient.Application.Features.Drafts.Commands.CreateDraft;
using GmailClient.Application.Features.Drafts.Commands.DeleteDraft;
using GmailClient.Application.Features.Drafts.Commands.UpdateDraft;
using GmailClient.Application.Features.Drafts.Queries.GetDraftDetails;
using GmailClient.Application.Features.Drafts.Queries.GetDraftsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {
        [Authorize]
        [HttpGet(Name = "GetAllDrafts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetDraftsListVm>>> GetAllDrafts()
        {
            var userId = currentUserService.UserId;
            var dtos = await mediator.Send(new GetDraftsListQuery() { UserId = userId });
            return Ok(dtos);
        }

        [Authorize]
        [HttpGet("{draftId}", Name = "GetDraftById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetDraftDetailsVm>> GetDraftsById(string draftId)
        {
            var userId = currentUserService.UserId;
            var dtos = await mediator.Send(new GetDraftDetailsQuery() 
            { 
                UserId = userId, 
                DraftId = draftId 
            });
            return Ok(dtos);
        }

        [Authorize]
        [HttpPost(Name = "CreateDraft")]
        public async Task<ActionResult> CreateDraft([FromBody] CreateDraftCommand createDraftCommand)
        {
            var userId = currentUserService.UserId;
            createDraftCommand.UserId = userId;
            await mediator.Send(createDraftCommand);

            return NoContent();
        }

        [Authorize]
        [HttpPut(Name = "UpdateDraft")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateDraftCommand updateDraftCommand)
        {
            var userId = currentUserService.UserId;
            updateDraftCommand.UserId = userId;
            await mediator.Send(updateDraftCommand);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{draftId}", Name = "DeleteDraft")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteDraft(string draftId)
        {
            var userId = currentUserService.UserId;
            await mediator.Send(new DeleteDraftCommand()
            {
                UserId = userId,
                DraftId = draftId
            });

            return NoContent();
        }
    }
}
