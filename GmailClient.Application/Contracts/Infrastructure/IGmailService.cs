using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface IGmailService
    {
        Task<GmailMessageResponse> GetAllMessagesAsync(string accessToken, string pageToken = null);
        Task SendEmailAsync(string accessToken, string to, string subject, string body);
        Task CreateDraftAsync(string accessToken, string to, string subject, string body);
        Task UpdateDraftAsync(string accessToken, string draftId, string to, string subject, string body);
        Task DeleteDraftAsync(string accessToken, string draftId);
        Task<List<DraftResponse>> GetDraftsAsync(string accessToken);
        Task<DraftResponse> GetDraftByIdAsync(string accessToken, string draftId);
    }
}
    