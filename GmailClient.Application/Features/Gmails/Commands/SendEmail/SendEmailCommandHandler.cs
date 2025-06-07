using GmailClient.Application.Contracts.Infrastructure;
using MediatR;

namespace GmailClient.Application.Features.Gmails.Commands.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
    {
        private readonly IGmailService _emailService;

        public SendEmailCommandHandler(IGmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAsync(request.AccessToken, request.To, request.Subject, request.Body);
            return Unit.Value;
        }
    }
}
