using GmailClient.Ui.Services;
using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IGmailDataService
    {
        Task<ApiResponse<MessagesListVm>> GetAllMessages(string pageToken = null);
        Task<ApiResponse<bool>> SendEmailAsync(SendEmailRequest sendEmailRequest);
    }
}
