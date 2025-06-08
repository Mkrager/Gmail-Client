using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Contracts.Services;
using GmailClient.Domain.Entities;
using MediatR;

namespace GmailClient.Application.Features.Gmails.Commands.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
    {
        private readonly IGmailService _emailService;
        private readonly IAsyncRepository<UserGmailToken> _userGmailTokenRepository;
        private readonly IAccessTokenManager _accessTokenManager;

        public SendEmailCommandHandler(IGmailService emailService, IAsyncRepository<UserGmailToken> userGmailTokenRepository, IAccessTokenManager accessTokenManager)
        {
            _emailService = emailService;
            _userGmailTokenRepository = userGmailTokenRepository;
            _accessTokenManager = accessTokenManager;
        }

        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            string accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);
            
            await _emailService.SendEmailAsync(accessToken, request.To, request.Subject, request.Body);

            return Unit.Value;
        }
    }
}
