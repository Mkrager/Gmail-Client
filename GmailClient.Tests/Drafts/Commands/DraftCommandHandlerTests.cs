using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Commands;
using GmailClient.Application.Features.Drafts.Commands.CreateDraft;
using GmailClient.Application.Features.Drafts.Commands.DeleteDraft;
using GmailClient.Application.Features.Drafts.Commands.UpdateDraft;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.Drafts.Commands
{
    public class DraftCommandHandlerTests
    {
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;

        public DraftCommandHandlerTests()
        {
            _gmailService = RepositoryMocks.GetGmailService();
            _accessTokenManager = RepositoryMocks.GetAccessTokenManager();
        }

        [Fact]
        public async Task CreateDraft_ShouldReturnUnitValue()
        {
            var handler = new DraftCommandHandler(_gmailService.Object, _accessTokenManager.Object);
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

        [Fact]
        public async Task DeleteDraft_ShouldReturnUnitValue()
        {
            var handler = new DraftCommandHandler(_gmailService.Object, _accessTokenManager.Object);
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

        [Fact]
        public async Task UpdateDraft_ShouldReturnUnitValue()
        {
            var handler = new DraftCommandHandler(_gmailService.Object, _accessTokenManager.Object);
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
