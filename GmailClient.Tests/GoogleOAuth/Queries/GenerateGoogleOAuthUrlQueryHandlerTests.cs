using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Features.GoogleOAuth.Queries.GenerateGoogleOAuthUrl;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.GoogleOAuth.Queries
{
    public class GenerateGoogleOAuthUrlQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IGoogleOAuthService> _googleOAuthService;
        private readonly Mock<IGoogleOAuthStateService> _googleOAuthStateService;
        private readonly Mock<IUserService> _userService;
        public GenerateGoogleOAuthUrlQueryHandlerTests()
        {
            _googleOAuthService = RepositoryMocks.GetGoogleOAuthService();
            _googleOAuthStateService = RepositoryMocks.GetGoogleOAuthStateService();
            _userService = RepositoryMocks.GetUserService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GenerateGoogleOAuth_ShouldReturnUrl()
        {
            var handler = new GenerateGoogleOAuthUrlQueryHandler(_googleOAuthService.Object, _googleOAuthStateService.Object);

            var result = await handler.Handle(new GenerateGoogleOAuthUrlQuery() { UserId = "userId" }, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenUserExsist()
        {
            var validator = new GenerateGoogleOAuthUrlQueryValidator(_userService.Object);
            var command = new GenerateGoogleOAuthUrlQuery()
            {
                UserId = "45353463324"
            };

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
        }

    }
}
