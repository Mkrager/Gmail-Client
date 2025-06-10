using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Features.GoogleOAuth.Commands.GoogleOAuthCallback;
using GmailClient.Application.Features.GoogleOAuth.Commands.oogleOAuthCallback;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.GoogleOAuth.Commands
{
    public class GoogleOAuthCallbackCommandHandlerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IGoogleOAuthService> _googleOAuthService;
        private readonly Mock<IGoogleOAuthStateService> _googleOAuthStateService;

        public GoogleOAuthCallbackCommandHandlerTests()
        {
            _googleOAuthService = RepositoryMocks.GetGoogleOAuthService();
            _googleOAuthStateService = RepositoryMocks.GetGoogleOAuthStateService();
            _mediator = RepositoryMocks.GetMediatorService();
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenStateIsValidAndTokensSavedSuccessfully()
        {
            var handler = new GoogleOAuthCallbackCommandHandler(_googleOAuthService.Object, _googleOAuthStateService.Object, _mediator.Object);

            var result = await handler.Handle(new GoogleOAuthCallbackCommand()
            {
                Code = "someCode",
                State = "someState"
            }, CancellationToken.None);

            Assert.True(result);
        }
    }
}
