using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IGmailDataService
    {
        Task<MessagesListVm> GetAllMessages(string pageToken = null);
        Task<bool> SendEmailAsync(SendEmailRequest sendEmailRequest);
    }
}
