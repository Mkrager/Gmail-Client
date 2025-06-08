using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.DTOs;
using Moq;

namespace GmailClient.Tests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IAuthenticationService> GetAuthenticationService()
        {
            var mockRepository = new Mock<IAuthenticationService>();

            mockRepository.Setup(repo => repo.AuthenticateAsync(It.IsAny<AuthenticationRequest>()))
                .ReturnsAsync(() =>
                {
                    var fakeResponse = new AuthenticationResponse
                    {
                        Token = "fake-token",
                        Email = "fake-email",
                        Id = "fake-id",
                        UserName = "fake-userName"
                    };
                    return fakeResponse;
                });

            mockRepository.Setup(repo => repo.RegisterAsync(It.IsAny<RegistrationRequest>()))
                .ReturnsAsync("some-id");

            return mockRepository;
        }
    }
}
