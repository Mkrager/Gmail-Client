using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface IGmailService
    {
        Task<GmailMessageResponse> GetAllMessagesAsync(string accessToken, string pageToken = null);
        Task SendEmailAsync(string accessToken, string to, string subject, string body);
    }
}
    