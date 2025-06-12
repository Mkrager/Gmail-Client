using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Commands.UpdateDraft;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.Drafts.Commands
{
    public class UpdateDraftCommandHandlerTests
    {
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;

        public UpdateDraftCommandHandlerTests()
        {
            _gmailService = RepositoryMocks.GetGmailService();
            _accessTokenManager = RepositoryMocks.GetAccessTokenManager();
        }

        [Fact]
        public async Task UpdateDraft_ShouldReturnUnitValue()
        {
            var handler = new UpdateDraftCommandHandler(_accessTokenManager.Object, _gmailService.Object);
            var command = new UpdateDraftCommand
            {
                DraftId = "draft123",
                Body = "Updated body",
                Subject = "Updated subject",
                To = "recipient@example.com",
                UserId = "user123"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenSubjectIsEmpty()
        {
            var validator = new UpdateDraftCommandValidator();
            var command = new UpdateDraftCommand
            {
                DraftId = "draft123",
                Body = "Some body",
                Subject = "", 
                To = "recipient@example.com",
                UserId = "user123"
            };

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Subject");
        }
    }
}
