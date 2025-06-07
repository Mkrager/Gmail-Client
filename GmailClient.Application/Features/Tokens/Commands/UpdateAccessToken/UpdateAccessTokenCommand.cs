using MediatR;

namespace GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken
{
    public class UpdateAccessTokenCommand : IRequest
    {
        public string refreshToken { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
    }
}
