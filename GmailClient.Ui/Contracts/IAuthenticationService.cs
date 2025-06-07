using GmailClient.Ui.Services;
using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<bool>> Login(LoginRequest request);
        Task<ApiResponse<bool>> Register(RegistrationRequest request);
        Task Logout();
        string GetAccessToken();
    }
}
