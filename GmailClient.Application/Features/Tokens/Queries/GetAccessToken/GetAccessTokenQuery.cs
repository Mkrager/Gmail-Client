using MediatR;

namespace GmailClient.Application.Features.Tokens.Queries.GetAccessToken
{
    public class GetAccessTokenQuery : IRequest<string>
    {
        public string refreshToken { get; set; } = string.Empty;
    }
}
