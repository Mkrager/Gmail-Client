using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.DTOs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;

namespace GmailClient.Infrastructure.Gmail
{
    public class GmailService : IGmailService
    {
        public async Task<List<GmailMessageDto>> GetAllMessagesAsync(string accessToken)
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            using var service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "My Gmail API App"
            });

            var messages = new List<GmailMessageDto>();

            var request = service.Users.Messages.List("me");
            string pageToken = null;

            do
            {
                request.PageToken = pageToken;
                var response = await request.ExecuteAsync();

                if (response.Messages != null)
                {
                    foreach (var msg in response.Messages)
                    {
                        var message = await service.Users.Messages.Get("me", msg.Id).ExecuteAsync(); // <-- _gmailService -> service

                        var headers = message.Payload.Headers;

                        string subject = headers.FirstOrDefault(h => h.Name == "Subject")?.Value ?? "";
                        string from = headers.FirstOrDefault(h => h.Name == "From")?.Value ?? "";
                        string date = headers.FirstOrDefault(h => h.Name == "Date")?.Value ?? "";

                        messages.Add(new GmailMessageDto
                        {
                            Id = msg.Id,
                            Subject = subject,
                            From = from,
                            Date = date
                        });
                    }
                }

                pageToken = response.NextPageToken;
            } while (pageToken != null);

            return messages;
        }
    }
}
