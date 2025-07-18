﻿namespace GmailClient.Application.DTOs
{
    public class GmailMessageDto
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsSent { get; set; }
        public bool IsInbox { get; set; }
    }
}
