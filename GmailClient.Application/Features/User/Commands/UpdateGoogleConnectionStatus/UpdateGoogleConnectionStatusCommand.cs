using MediatR;

namespace GmailClient.Application.Features.User.Commands.UpdateGoogleConnectionStatus
{
    public class UpdateGoogleConnectionStatusCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
    }
}
