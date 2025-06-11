using GmailClient.Ui.Services;
using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IUserDataService
    {
        Task<ApiResponse<UserDetailsResponse>> GetUserDetails();
    }
}
