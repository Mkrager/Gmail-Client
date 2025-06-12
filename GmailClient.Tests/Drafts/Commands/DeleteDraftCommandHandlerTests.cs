using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Commands.DeleteDraft;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.Drafts.Commands
{
    public class DeleteDraftCommandHandlerTests
    {
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;

        public DeleteDraftCommandHandlerTests()
        {
            _gmailService = RepositoryMocks.GetGmailService();
            _accessTokenManager = RepositoryMocks.GetAccessTokenManager();
        }

        [Fact]
        public async Task DeleteDraft_ShouldReturnUnitValue()
        {
            var handler = new DeleteDraftCommandHandler(_gmailService.Object, _accessTokenManager.Object);
            var command = new DeleteDraftCommand
            {
                UserId = "user123",
                DraftId = "draft123"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenUserdIsEmpty()
        {
            var validator = new DeleteDraftCommandValidator();
            var command = new DeleteDraftCommand
            {
                UserId = "",
            };

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserId");
        }
    }
}
