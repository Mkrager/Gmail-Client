using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IGmailDataService
    {
        Task<List<MessagesListVm>> GetAllMessages();
    }
}
