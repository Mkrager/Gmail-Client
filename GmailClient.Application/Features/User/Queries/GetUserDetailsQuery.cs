using MediatR;

namespace GmailClient.Application.Features.User.Queries
{
    public class GetUserDetailsQuery : IRequest<UserDetailsVm>
    {
        public string Id { get; set; } = string.Empty;
    }
}
