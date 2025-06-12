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
        private Google.Apis.Gmail.v1.GmailService CreateGmailService(string accessToken)
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            return new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "EmailSender"
            });
        }

        private string GetHeaderValue(IList<MessagePartHeader> headers, string name)
            => headers.FirstOrDefault(h => h.Name == name)?.Value ?? string.Empty;

        private string GetBodyFromParts(IList<MessagePart> parts)
        {
            foreach (var part in parts)
            {
                if (part.MimeType == "text/plain" || part.MimeType == "text/html")
                {
                    if (part.Body?.Data != null)
                        return Base64UrlDecode(part.Body.Data);
                }
                if (part.Parts != null && part.Parts.Count > 0)
                {
                    var body = GetBodyFromParts(part.Parts);
                    if (!string.IsNullOrEmpty(body))
                        return body;
                }
            }
            return string.Empty;
        }

        private string GetMessageBody(Message message)
        {
            if (message.Payload.Body?.Data != null)
                return Base64UrlDecode(message.Payload.Body.Data);

            if (message.Payload.Parts != null)
                return GetBodyFromParts(message.Payload.Parts);

            return string.Empty;
        }

        private string Base64UrlEncode(byte[] input)
            => Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_").Replace("=", "");

        private string Base64UrlDecode(string input)
        {
            var cleaned = input.Replace("-", "+").Replace("_", "/");
            var bytes = Convert.FromBase64String(cleaned);
            return Encoding.UTF8.GetString(bytes);
        }

        public async Task<GmailMessageResponse> GetAllMessagesAsync(string accessToken, string pageToken = null)
        {
            using var service = CreateGmailService(accessToken);
            var request = service.Users.Messages.List("me");
            request.MaxResults = 10;
            request.PageToken = pageToken;

            var response = await request.ExecuteAsync();
            var gmailResponse = new GmailMessageResponse();

            if (response.Messages != null)
            {
                foreach (var msg in response.Messages)
                {
                    var message = await service.Users.Messages.Get("me", msg.Id).ExecuteAsync();
                    var headers = message.Payload.Headers;

                    gmailResponse.Messages.Add(new GmailMessageDto
                    {
                        Id = msg.Id,
                        Subject = GetHeaderValue(headers, "Subject"),
                        From = GetHeaderValue(headers, "From"),
                        Date = GetHeaderValue(headers, "Date"),
                        Body = GetMessageBody(message),
                        IsSent = message.LabelIds.Contains("SENT"),
                        IsInbox = message.LabelIds.Contains("INBOX")
                    });
                }
            }

            gmailResponse.NextPageToken = response.NextPageToken;
            return gmailResponse;
        }

        public async Task SendEmailAsync(string accessToken, string to, string subject, string body)
        {
            using var service = CreateGmailService(accessToken);

            var profile = await service.Users.GetProfile("me").ExecuteAsync();
            string userEmail = profile.EmailAddress;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(userEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(to.Trim());

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);
            using var stream = new MemoryStream();
            await mimeMessage.WriteToAsync(stream);
            var rawMessage = Base64UrlEncode(stream.ToArray());

            var message = new Message { Raw = rawMessage };

            await service.Users.Messages.Send(message, "me").ExecuteAsync();
        }

        public async Task CreateDraftAsync(string accessToken, string to, string subject, string body)
        {
            using var service = CreateGmailService(accessToken);

            var profile = await service.Users.GetProfile("me").ExecuteAsync();
            string userEmail = profile.EmailAddress;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(userEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(to.Trim());

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            using var stream = new MemoryStream();
            await mimeMessage.WriteToAsync(stream);

            var rawMessage = Base64UrlEncode(stream.ToArray());
            var message = new Message { Raw = rawMessage };
            var draft = new Draft { Message = message };

            await service.Users.Drafts.Create(draft, "me").ExecuteAsync();
        }

        public async Task UpdateDraftAsync(string accessToken, string draftId, string to, string subject, string body)
        {
            using var service = CreateGmailService(accessToken);

            var profile = await service.Users.GetProfile("me").ExecuteAsync();
            string userEmail = profile.EmailAddress;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(userEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(to.Trim());

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            using var stream = new MemoryStream();
            await mimeMessage.WriteToAsync(stream);

            var rawMessage = Base64UrlEncode(stream.ToArray());
            var message = new Message { Raw = rawMessage };

            var draft = new Draft
            {
                Id = draftId,
                Message = message
            };

            await service.Users.Drafts.Update(draft, "me", draftId).ExecuteAsync();
        }
        public async Task DeleteDraftAsync(string accessToken, string draftId)
        {
            using var service = CreateGmailService(accessToken);

            await service.Users.Drafts.Delete("me", draftId).ExecuteAsync();
        }

        public async Task<List<DraftResponse>> GetDraftsAsync(string accessToken)
        {
            using var service = CreateGmailService(accessToken);

            var request = service.Users.Drafts.List("me");

            var response = await request.ExecuteAsync();
            var drafts = response.Drafts ?? new List<Draft>();
            var result = new List<DraftResponse>();

            foreach (var draft in drafts)
            {
                var fullDraft = await service.Users.Drafts.Get("me", draft.Id).ExecuteAsync();
                var message = fullDraft.Message;
                var headers = message.Payload.Headers;

                string GetHeader(string name) => headers.FirstOrDefault(h => h.Name == name)?.Value ?? string.Empty;

                string GetBody()
                {
                    if (message.Payload.Body?.Data != null)
                        return Base64UrlDecode(message.Payload.Body.Data);

                    if (message.Payload.Parts != null)
                        return GetBodyFromParts(message.Payload.Parts);

                    return string.Empty;
                }

                result.Add(new DraftResponse
                {
                    DraftId = fullDraft.Id,
                    MessageId = message.Id,
                    From = GetHeader("From"),
                    To = GetHeader("To"),
                    Subject = GetHeader("Subject"),
                    Date = GetHeader("Date"),
                    Body = GetBody()
                });
            }

            return result;
        }

        public async Task<DraftResponse> GetDraftByIdAsync(string accessToken, string draftId)
        {
            using var service = CreateGmailService(accessToken);

            var draft = await service.Users.Drafts.Get("me", draftId).ExecuteAsync();
            var message = draft.Message;
            var headers = message.Payload.Headers;

            string GetHeader(string name) => headers.FirstOrDefault(h => h.Name == name)?.Value ?? string.Empty;

            string GetBody()
            {
                if (message.Payload.Body?.Data != null)
                    return Base64UrlDecode(message.Payload.Body.Data);

                if (message.Payload.Parts != null)
                    return GetBodyFromParts(message.Payload.Parts);

                return string.Empty;
            }

            return new DraftResponse
            {
                DraftId = draft.Id,
                MessageId = message.Id,
                From = GetHeader("From"),
                To = GetHeader("To"),
                Subject = GetHeader("Subject"),
                Date = GetHeader("Date"),
                Body = GetBody()
            };
        }
    }
}
