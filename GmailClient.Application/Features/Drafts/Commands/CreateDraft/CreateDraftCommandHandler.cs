using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Commands.CreateDraft
{
    public class CreateDraftCommandHandler : IRequestHandler<CreateDraftCommand>
    {
        private readonly IGmailService _gmailService;
        private readonly IAccessTokenManager _accessTokenManager;
        public CreateDraftCommandHandler(IGmailService gmailService, IAccessTokenManager accessTokenManager)
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
    }
}
