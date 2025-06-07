using MediatR;

namespace GmailClient.Application.Features.Gmails.Commands.SendEmail
{
    public class SendEmailCommand : IRequest
    {
        public string AccessToken { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
