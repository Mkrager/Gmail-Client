﻿namespace GmailClient.Application.DTOs
{
    public class UserDetailsResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsGoogleConnected { get; set; }
    }
}
