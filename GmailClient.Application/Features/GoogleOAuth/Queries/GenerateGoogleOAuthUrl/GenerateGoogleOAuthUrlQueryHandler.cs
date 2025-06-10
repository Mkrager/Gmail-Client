using GmailClient.Application.Contracts.Infrastructure;
using MediatR;

namespace GmailClient.Application.Features.GoogleOAuth.Queries.GenerateGoogleOAuthUrl
{
    public class GenerateGoogleOAuthUrlQueryHandler : IRequestHandler<GenerateGoogleOAuthUrlQuery, string>
    {
        private readonly IGoogleOAuthService _googleOAuthService;
        private readonly IGoogleOAuthStateService _googleOAuthStateService;

        public GenerateGoogleOAuthUrlQueryHandler(IGoogleOAuthService googleOAuthService, IGoogleOAuthStateService googleOAuthStateService)
        {
            _googleOAuthService = googleOAuthService;
            _googleOAuthStateService = googleOAuthStateService;
        }
        public Task<string> Handle(GenerateGoogleOAuthUrlQuery request, CancellationToken cancellationToken)
        {
            var state = Guid.NewGuid().ToString("N");
            _googleOAuthStateService.StoreState(state, request.UserId, TimeSpan.FromMinutes(10));
            var url = _googleOAuthService.GenerateGoogleAuthorizationUrl(state);
            return Task.FromResult(url);
        }
    }
}
