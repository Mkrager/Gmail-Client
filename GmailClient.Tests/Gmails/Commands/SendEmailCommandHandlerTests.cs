using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Account.Commands.Registration;
using GmailClient.Application.Features.Gmails.Commands.SendEmail;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.Gmails.Commands
{
    public class SendEmailCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;

        public SendEmailCommandHandlerTests()
        {
            _gmailService = RepositoryMocks.GetGmailService();
            _accessTokenManager = RepositoryMocks.GetAccessTokenManager();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task SendEmail_ShouldNotReturn()
        {
            var handler = new SendEmailCommandHandler(_gmailService.Object, _accessTokenManager.Object);

            var result = await handler.Handle(new SendEmailCommand() 
            { 
                Body = "TestBody",
                Subject = "TestSubject",
                To = "testEmail@email.com"

            }, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenBodyEmpty()
        {
            var validator = new SendEmailCommandValidator();
            var query = new SendEmailCommand
            {
                Body = "",
                Subject = "TestSubject",
                To = "testEmail@email.com"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Body");
        }

    }
}
