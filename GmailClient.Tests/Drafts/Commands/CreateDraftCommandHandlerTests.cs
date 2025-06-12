using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Commands.CreateDraft;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.Drafts.Commands
{
    public class CreateDraftCommandHandlerTests
    {
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;

        public CreateDraftCommandHandlerTests()
        {
            _gmailService = RepositoryMocks.GetGmailService();
            _accessTokenManager = RepositoryMocks.GetAccessTokenManager();
        }

        [Fact]
        public async Task CreateDraft_ShouldReturnUnitValue()
        {
            var handler = new CreateDraftCommandHandler(_gmailService.Object, _accessTokenManager.Object);
            var command = new CreateDraftCommand
            {
                Body = "Draft body",
                Subject = "Draft subject",
                To = "recipient@example.com",
                UserId = "user123"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenBodyIsEmpty()
        {
            var validator = new CreateDraftCommandValidator();
            var command = new CreateDraftCommand
            {
                Body = "",
                Subject = "Some subject",
                To = "recipient@example.com",
                UserId = "user123"
            };

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Body");
        }
    }
}
