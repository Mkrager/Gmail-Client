using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IUserDataService
    {
        Task<UserDetailsResponse> GetUserDetails(string userId);
    }
}
