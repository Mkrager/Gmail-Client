using MediatR;

namespace GmailClient.Application.Features.Gmails.Commands.SendEmail
{
    public class SendEmailCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
