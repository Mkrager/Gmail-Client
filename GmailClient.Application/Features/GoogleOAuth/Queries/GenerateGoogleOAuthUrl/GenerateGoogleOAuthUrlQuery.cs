using MediatR;

namespace GmailClient.Application.Features.GoogleOAuth.Queries.GenerateGoogleOAuthUrl
{
    public class GenerateGoogleOAuthUrlQuery : IRequest<string>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
