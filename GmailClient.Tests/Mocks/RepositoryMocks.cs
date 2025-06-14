using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Features.User.Commands.UpdateGoogleConnectionStatus;
using GmailClient.Domain.Entities;
using MediatR;
using Moq;
using System.Net;

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

            var drafts = new List<DraftResponse>
            {
                new DraftResponse()
                {
                    DraftId = "draft1",
                    MessageId = "msg1",
                    From = "alice@example.com",
                    To = "bob@example.com",
                    Subject = "Meeting Reminder",
                    Date = "2025-06-12T10:00:00Z",
                    Body = "Body1"
                },
                new DraftResponse()
                {
                    DraftId = "draft2",
                    MessageId = "msg2",
                    From = "carol@example.com",
                    To = "dave@example.com",
                    Subject = "Project Update",
                    Date = "2025-06-11T14:30:00Z",
                    Body = "Body2"
                },
                new DraftResponse()
                {
                    DraftId = "draft3",
                    MessageId = "msg3",
                    From = "eve@example.com",
                    To = "frank@example.com",
                    Subject = "Invitation",
                    Date = "2025-06-10T09:15:00Z",
                    Body = "Body3"
                },
                new DraftResponse()
                {
                    DraftId = "draft4",
                    MessageId = "msg4",
                    From = "grace@example.com",
                    To = "heidi@example.com",
                    Subject = "Follow-up",
                    Date = "2025-06-09T16:45:00Z",
                    Body = "Body4"
                }
            };


            mockService.Setup(service => service.GetAllMessagesAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new GmailMessageResponse
                {
                    Messages = messageList,
                    NextPageToken = "sometoken"
                });

            mockService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockService.Setup(service => service.GetDraftsAsync(It.IsAny<string>()))
                .ReturnsAsync(drafts);

            mockService.Setup(service => service.GetDraftByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string userId, string draftId) => drafts.FirstOrDefault(a => a.DraftId == draftId));

            mockService.Setup(service => service.CreateDraftAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockService.Setup(service => service.UpdateDraftAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockService.Setup(service => service.DeleteDraftAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);


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

        public static Mock<IUserService> GetUserService()
        {
            var users = new List<UserDetailsResponse>()
            {
                new UserDetailsResponse()
                {
                    Id = "52543534534",
                    Email = "email@gmail.com",
                    UserName = "TestUserName"
                }
            };

            var mockService = new Mock<IUserService>();

            mockService.Setup(service => service.GetUserDetails(It.IsAny<string>()))
                .ReturnsAsync((string userId) => users.FirstOrDefault(x => x.Id == userId));

            mockService.Setup(service => service.SetGoogleConnectedAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            mockService.Setup(service => service.IsGoogleConnected(It.IsAny<string>()))
                .ReturnsAsync(true);

            return mockService;
        }
        public static Mock<ITokenEncryptionService> GetTokenEncryptionService()
        {
            var mockService = new Mock<ITokenEncryptionService>();

            mockService.Setup(service => service.Encrypt(It.IsAny<string>()))
                .Returns("encrypt-token");

            mockService.Setup(service => service.Decrypt(It.IsAny<string>()))
                .Returns("decrypt-token");

            return mockService;
        }

        public static Mock<IGoogleOAuthService> GetGoogleOAuthService()
        {
            var mockService = new Mock<IGoogleOAuthService>();

            mockService.Setup(service => service.ExchangeCodeForTokensAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new GoogleTokenResponse
                {
                    RefreshToken = "someRefreshTokens",
                    AccessToken = "someAccessToken",
                    ExpiresIn = 3000,
                    IdToken = "someId",
                    TokenType = "someTokenType"
                });

            mockService.Setup(service => service.GenerateGoogleAuthorizationUrl(It.IsAny<string>()))
                .Returns("someUrl");

            return mockService;
        }

        public static Mock<IGoogleOAuthStateService> GetGoogleOAuthStateService()
        {
            var mockService = new Mock<IGoogleOAuthStateService>();

            string outUserId = "someUserId";

            mockService.Setup(service => service.TryGetUserIdByState(It.IsAny<string>(), out outUserId))
                .Returns(true);

            mockService.Setup(service => service.StoreState(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            return mockService;
        }

        public static Mock<IMediator> GetMediatorService()
        {
            var mediator = new Mock<IMediator>();

            mediator
                .Setup(m => m.Send(It.IsAny<SaveTokensCommand>(), default))
                .ReturnsAsync(Guid.NewGuid());
            mediator
                .Setup(m => m.Send(It.IsAny<UpdateGoogleConnectionStatusCommand>(), default))
                .ReturnsAsync(Unit.Value);

            return mediator;
        }
    }
}
