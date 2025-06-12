using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Commands.UpdateDraft
{
    public class UpdateDraftCommandHandler : IRequestHandler<UpdateDraftCommand>
    {
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IGmailService _gmailService;

        public UpdateDraftCommandHandler(IAccessTokenManager accessTokenManager, IGmailService gmailService)
        {
            _accessTokenManager = accessTokenManager;
            _gmailService = gmailService;
        }
        public async Task<Unit> Handle(UpdateDraftCommand request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            await _gmailService.UpdateDraftAsync(
                accessToken, request.DraftId, request.To, request.Subject, request.Body);

            return Unit.Value;
        }
    }
}
