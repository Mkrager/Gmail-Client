namespace GmailClient.Application.Features.User.Queries
{
    public class UserDetailsVm
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsGoogleConnected { get; set; }
    }
}
