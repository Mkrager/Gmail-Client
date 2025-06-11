namespace GmailClient.Ui.Contracts
{
    public interface IGoogleOAuthDataService
    {
        Task<string> GetGoogleSignInUrlAsync();
    }
}
