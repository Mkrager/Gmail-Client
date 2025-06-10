namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface ITokenEncryptionService
    {
        string Encrypt(string token);
        string Decrypt(string encryptedToken);
    }
}
