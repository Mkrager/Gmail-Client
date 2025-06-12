using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Commands.DeleteDraft
{
    public class DeleteDraftCommandHandler : IRequestHandler<DeleteDraftCommand>
    {
        private readonly IGmailService _gmailService;
        private readonly IAccessTokenManager _accessTokenManager;
        public DeleteDraftCommandHandler(IGmailService gmailService, IAccessTokenManager accessTokenManager)
        {
            _gmailService = gmailService;
            _accessTokenManager = accessTokenManager;
        }
        public async Task<Unit> Handle(DeleteDraftCommand request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            await _gmailService.DeleteDraftAsync(accessToken, request.DraftId);

            return Unit.Value;
        }
    }
}
