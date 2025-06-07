namespace GmailClient.Application.Exceptions
{
    public class AccessTokenRefreshFailedException : Exception
    {
        public AccessTokenRefreshFailedException(string message) : base(message) { }
    }
}
