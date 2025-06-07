using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Exceptions;
using GmailClient.Domain.Entities;
using MediatR;

namespace GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken
{
    public class UpdateAccessTokenCommandHandler : IRequestHandler<UpdateAccessTokenCommand>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserGmailTokenRepository _userGmailTokenRepository;

        public UpdateAccessTokenCommandHandler(ITokenService tokenService, IUserGmailTokenRepository userGmailTokenRepository)
        {
            _tokenService = tokenService;
            _userGmailTokenRepository = userGmailTokenRepository;
        }

        public async Task<Unit> Handle(UpdateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var newAccessToken = await _tokenService.GetAccessTokenAsync(request.refreshToken);

            if (newAccessToken.AccessToken == null)
            {
                throw new AccessTokenRefreshFailedException("Failed to refresh access token using the provided refresh token.");
            }

            await _userGmailTokenRepository.UpdateAccessTokenAsync(request.userId, newAccessToken.AccessToken, newAccessToken.ExpiresAt);

            return Unit.Value;
        }
    }
}
