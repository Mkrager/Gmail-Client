using AutoMapper;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Profiles;
using GmailClient.Domain.Entities;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.Tokens.Commands
{
    public class SaveTokensCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<UserGmailToken>> _userGmailTokenRepository;
        public SaveTokensCommandHandlerTests()
        {
            _userGmailTokenRepository = RepositoryMocks.GetUserGmailTokenRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Should_Add_Tokens_Successfully()
        {
            var handler = new SaveTokensCommandHandler(_userGmailTokenRepository.Object, _mapper);

            var command = new SaveTokensCommand()
            {
                AccessToken = "653634563563",
                ExpiresAt = 3600,
                RefreshToken = "4234523534563",
                UserId = "45353463324"               
            };

            var result = handler.Handle(command, CancellationToken.None);

            var allUserGmailTokens = await _userGmailTokenRepository.Object.ListAllAsync();

            var addedTokens = allUserGmailTokens.FirstOrDefault(a => a.UserId == command.UserId);

            Assert.Equal(2, allUserGmailTokens.Count);
            Assert.Equal(addedTokens.AccessToken, command.AccessToken);
            Assert.Equal(addedTokens.RefreshToken, command.RefreshToken);
            var expectedExpiration = DateTime.UtcNow.AddSeconds(command.ExpiresAt);
            Assert.True((addedTokens.ExpiresAt - expectedExpiration).TotalMilliseconds < 200);
            Assert.Equal(addedTokens.UserId, command.UserId);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenAsseccTokenEmpty()
        {
            var validator = new SaveTokensCommandValidator();
            var command = new SaveTokensCommand()
            {
                AccessToken = "",
                ExpiresAt = 3600,
                RefreshToken = "4234523534563",
                UserId = "45353463324"
            };

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "AccessToken");
        }

    }
}
