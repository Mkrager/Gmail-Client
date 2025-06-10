using MediatR;

namespace GmailClient.Application.Features.GoogleOAuth.Commands.GoogleOAuthCallback
{
    public class GoogleOAuthCallbackCommand : IRequest<bool>
    {
        public string Code { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
