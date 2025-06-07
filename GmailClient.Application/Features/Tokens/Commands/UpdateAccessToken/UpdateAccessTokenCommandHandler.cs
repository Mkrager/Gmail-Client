using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Domain.Entities;
using MediatR;

namespace GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken
{
    public class UpdateAccessTokenCommandHandler : IRequestHandler<UpdateAccessTokenCommand>
    {
        private readonly ITokenService _tokenService;
        private readonly IAsyncRepository<UserGmailToken> _userGmailTokenRepository;

        public UpdateAccessTokenCommandHandler(ITokenService tokenService, IAsyncRepository<UserGmailToken> userGmailTokenRepository)
        {
            _tokenService = tokenService;
            _userGmailTokenRepository = userGmailTokenRepository;
        }

        public async Task<Unit> Handle(UpdateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var newAccessToken = await _tokenService.GetAccessTokenAsync(request.refreshToken);

            if (newAccessToken == null)
            {
                
            }

            await _userGmailTokenRepository.UpdateAsync();
        }
    }
}
