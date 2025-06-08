using MediatR;

namespace GmailClient.Application.Features.Tokens.Commands.SaveTokens
{
    public class SaveTokensCommand : IRequest<Guid>
    {
        public string UserId { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresAt { get; set; }
    }
}
