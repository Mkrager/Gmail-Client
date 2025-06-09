using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.DTOs;
using GmailClient.Domain.Entities;
using Moq;

namespace GmailClient.Tests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IAuthenticationService> GetAuthenticationService()
        {
            var mockService = new Mock<IAuthenticationService>();

            mockService.Setup(service => service.AuthenticateAsync(It.IsAny<AuthenticationRequest>()))
                .ReturnsAsync(new AuthenticationResponse
                {
                    Token = "fake-token",
                    Email = "fake-email",
                    Id = "fake-id",
                    UserName = "fake-userName"
                });


            mockService.Setup(service => service.RegisterAsync(It.IsAny<RegistrationRequest>()))
                .ReturnsAsync("some-id");

            return mockService;
        }
        public static Mock<IAccessTokenManager> GetAccessTokenManager()
        {
            var mockService = new Mock<IAccessTokenManager>();

            mockService.Setup(service => service.GetValidAccessTokenAsync(It.IsAny<string>()))
                .ReturnsAsync("7356783478905");

            return mockService;
        }

        public static Mock<IGmailService> GetGmailService()
        {
            var mockService = new Mock<IGmailService>();

            var messageList = new List<GmailMessageDto>()
{
                new GmailMessageDto()
                {
                    Id = "7d8a7f8e-3e8e-4b18-9a2f-23b8e77a1c4f",
                    Body = "body",
                    Date = "somedate",
                    From = "email@gmail.com",
                    IsInbox = true,
                    IsSent = false,
                    Subject = "subject"
                },
                new GmailMessageDto()
                {
                    Id = "bb5d5e43-fbe2-4f06-beb2-7d5cc7db17de",
                    Body = "second body",
                    Date = "2025-06-08T10:00:00",
                    From = "user1@example.com",
                    IsInbox = false,
                    IsSent = true,
                    Subject = "Second Subject"
                },
                new GmailMessageDto()
                {
                    Id = "c1839d32-cb3f-42cf-93a1-cd9629d6c749",
                    Body = "third body",
                    Date = "2025-06-08T11:00:00",
                    From = "user2@example.com",
                    IsInbox = true,
                    IsSent = false,
                    Subject = "Third Subject"
                },
                new GmailMessageDto()
                {
                    Id = "62b99108-d42c-4eb4-bbaf-847118d86e7f",
                    Body = "fourth body",
                    Date = "2025-06-08T12:00:00",
                    From = "user3@example.com",
                    IsInbox = true,
                    IsSent = true,
                    Subject = "Fourth Subject"
                },
                new GmailMessageDto()
                {
                    Id = "af8a9b2b-016f-4a68-92bc-4b6fa9d576f4",
                    Body = "fifth body",
                    Date = "2025-06-08T13:00:00",
                    From = "user4@example.com",
                    IsInbox = false,
                    IsSent = true,
                    Subject = "Fifth Subject"
                }
            };

            mockService.Setup(service => service.GetAllMessagesAsync(It.IsAny<string>()))
                .ReturnsAsync(messageList);

            mockService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            return mockService;
        }

        public static Mock<IAsyncRepository<UserGmailToken>> GetUserGmailTokenRepository()
        {
            var mockRepository = new Mock<IAsyncRepository<UserGmailToken>>();

            var userGmailTokens = new List<UserGmailToken>()
            {
                new UserGmailToken()
                {
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    AccessToken = "4234323423423",
                    RefreshToken = "34534534534",
                    UserId = "45674564645",
                    Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479")
                }
            };

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<UserGmailToken>()))
                .ReturnsAsync((UserGmailToken userGmailToken) =>
                {
                    userGmailTokens.Add(userGmailToken);
                    return userGmailToken;
                });

            mockRepository.Setup(repo => repo.ListAllAsync())
                .ReturnsAsync(userGmailTokens);

            return mockRepository;
        }
    }
}
