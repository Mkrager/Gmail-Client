using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Commands.CreateDraft;
using GmailClient.Application.Features.Drafts.Commands.DeleteDraft;
using GmailClient.Application.Features.Drafts.Commands.UpdateDraft;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Commands
{
    public class DraftCommandHandler :
        IRequestHandler<CreateDraftCommand>,
        IRequestHandler<UpdateDraftCommand>,
        IRequestHandler<DeleteDraftCommand>
    {
        private readonly IGmailService _gmailService;
        private readonly IAccessTokenManager _accessTokenManager;
        public DraftCommandHandler(IGmailService gmailService, IAccessTokenManager accessTokenManager)
        {
            _gmailService = gmailService;
            _accessTokenManager = accessTokenManager;
        }

        public async Task<Unit> Handle(CreateDraftCommand request, CancellationToken cancellationToken)
        {
            string accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            await _gmailService.CreateDraftAsync(accessToken, request.To, request.Subject, request.Body);

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateDraftCommand request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            await _gmailService.UpdateDraftAsync(
                accessToken, request.DraftId, request.To, request.Subject, request.Body);

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteDraftCommand request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            await _gmailService.DeleteDraftAsync(accessToken, request.DraftId);

            return Unit.Value;
        }
    }
}
