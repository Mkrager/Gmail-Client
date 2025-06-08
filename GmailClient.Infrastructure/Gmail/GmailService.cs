using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.DTOs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using System.Net.Mail;
using System.Text;

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
                ApplicationName = "EmailSender"
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
                        var message = await service.Users.Messages.Get("me", msg.Id).ExecuteAsync();

                        var headers = message.Payload.Headers;

                        string subject = headers.FirstOrDefault(h => h.Name == "Subject")?.Value ?? "";
                        string from = headers.FirstOrDefault(h => h.Name == "From")?.Value ?? "";
                        string date = headers.FirstOrDefault(h => h.Name == "Date")?.Value ?? "";
                        string body = GetMessageBody(message);
                        bool isSent = message.LabelIds.Contains("SENT");
                        bool isInbox = message.LabelIds.Contains("INBOX");

                        messages.Add(new GmailMessageDto
                        {
                            Id = msg.Id,
                            Subject = subject,
                            From = from,
                            Date = date,
                            Body = body,
                            IsSent = isSent,
                            IsInbox = isInbox,
                        });
                    }
                }

                pageToken = response.NextPageToken;
            } while (pageToken != null);

            return messages;
        }

        public async Task SendEmailAsync(string accessToken, string to, string subject, string body)
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            using var service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "EmailSender"
            });

            var profile = await service.Users.GetProfile("me").ExecuteAsync();
            string userEmail = profile.EmailAddress;

            var mailMessage = new MailMessage();
            mailMessage.To.Add(to.Trim());
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;
            mailMessage.From = new MailAddress(userEmail);

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            using var stream = new MemoryStream();
            await mimeMessage.WriteToAsync(stream);
            var rawMessage = Convert.ToBase64String(stream.ToArray())
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            var message = new Message
            {
                Raw = rawMessage
            };

            await service.Users.Messages.Send(message, "me").ExecuteAsync();
        }

        private string GetMessageBody(Message message)
        {
            if (message.Payload.Body?.Data != null)
            {
                return DecodeBase64(message.Payload.Body.Data);
            }

            if (message.Payload.Parts != null)
            {
                foreach (var part in message.Payload.Parts)
                {
                    if (part.MimeType == "text/plain" || part.MimeType == "text/html")
                    {
                        if (part.Body?.Data != null)
                            return DecodeBase64(part.Body.Data);
                    }

                    if (part.Parts != null)
                    {
                        foreach (var subPart in part.Parts)
                        {
                            if (subPart.MimeType == "text/plain" || subPart.MimeType == "text/html")
                            {
                                if (subPart.Body?.Data != null)
                                    return DecodeBase64(subPart.Body.Data);
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }


        private string DecodeBase64(string base64Data)
        {
            var cleaned = base64Data.Replace("-", "+").Replace("_", "/");
            var bytes = Convert.FromBase64String(cleaned);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
