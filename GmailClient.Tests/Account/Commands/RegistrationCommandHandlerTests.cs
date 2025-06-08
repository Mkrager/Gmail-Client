using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.Features.Account.Commands.Registration;
using GmailClient.Application.Features.Account.Queries.Authentication;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.Account.Commands
{
    public class RegistrationCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAuthenticationService> _authenticationService;

        public RegistrationCommandHandlerTests()
        {
            _authenticationService = RepositoryMocks.GetAuthenticationService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_WithValidValues_ReturnsUserId()
        {
            var handler = new RegistrationCommandHandler(_authenticationService.Object, _mapper);

            var registrationQuery = new RegistrationCommand()
            {
                Email = "email@gmail.com",
                Password = "password",
                FirstName = "testName",
                LastName = "testLast",
                UserName = "testUsername"
            };

            var result = await handler.Handle(registrationQuery, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenEmailEmpty()
        {
            var validator = new RegistrationCommandValidator();
            var query = new RegistrationCommand
            {
                Email = "",
                Password = "password",
                FirstName = "testName",
                LastName = "testLast",
                UserName = "testUsername"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Email");
        }
    }
}
