using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface IGmailService
    {
        Task<List<GmailMessageDto>> GetAllMessagesAsync(string accessToken);
        Task SendEmailAsync(string accessToken, string to, string subject, string body);
    }
}
    