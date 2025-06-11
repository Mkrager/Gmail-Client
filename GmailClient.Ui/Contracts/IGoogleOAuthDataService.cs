using GmailClient.Ui.Services;

namespace GmailClient.Ui.Contracts
{
    public interface IGoogleOAuthDataService
    {
        Task<ApiResponse<string>> GetGoogleSignInUrlAsync();
    }
}
