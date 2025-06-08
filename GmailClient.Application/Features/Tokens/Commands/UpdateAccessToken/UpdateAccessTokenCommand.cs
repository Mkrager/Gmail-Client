using MediatR;

namespace GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken
{
    public class UpdateAccessTokenCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
    }
}
