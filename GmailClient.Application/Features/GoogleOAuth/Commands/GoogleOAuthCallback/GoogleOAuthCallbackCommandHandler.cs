using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Features.GoogleOAuth.Commands.GoogleOAuthCallback;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Features.User.Commands.UpdateGoogleConnectionStatus;
using GmailClient.Domain.Entities;
using MediatR;

namespace GmailClient.Application.Features.GoogleOAuth.Commands.oogleOAuthCallback
{
    public class GoogleOAuthCallbackCommandHandler : IRequestHandler<GoogleOAuthCallbackCommand, bool>
    {
        private readonly IGoogleOAuthService _googleOAuthService;
        private readonly IGoogleOAuthStateService _googleOAuthStateService;
        private readonly IMediator _mediator;

        public GoogleOAuthCallbackCommandHandler(
            IGoogleOAuthService googleOAuthService,
            IGoogleOAuthStateService googleOAuthStateService,
            IMediator mediator)
        {
            _googleOAuthService = googleOAuthService;
            _googleOAuthStateService = googleOAuthStateService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(GoogleOAuthCallbackCommand request, CancellationToken cancellationToken)
        {
            if (!_googleOAuthStateService.TryGetUserIdByState(request.State, out string userId))
            {
                return false;
            }

            var redirectUri = "https://localhost:7075/api/googleoauth/google-callback";
            var tokenInfo = await _googleOAuthService.ExchangeCodeForTokensAsync(request.Code, redirectUri);

            await _mediator.Send(new SaveTokensCommand
            {
                AccessToken = tokenInfo.Access_token,
                ExpiresAt = tokenInfo.Expires_in,
                RefreshToken = tokenInfo.Refresh_token,
                UserId = userId
            });

            await _mediator.Send(new UpdateGoogleConnectionStatusCommand
            {
                IsConnected = true,
                UserId = userId
            });

            return true;
        }
    }
}

