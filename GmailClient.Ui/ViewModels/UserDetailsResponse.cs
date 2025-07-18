﻿namespace GmailClient.Ui.ViewModels
{
    public class UserDetailsResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsGoogleConnected { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
