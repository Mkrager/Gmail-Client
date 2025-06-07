using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> RegisterAsync(RegistrationRequest request);
    }
}
