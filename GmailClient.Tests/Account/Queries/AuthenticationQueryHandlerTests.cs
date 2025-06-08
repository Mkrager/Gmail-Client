using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Account.Queries.Authentication;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.Account.Queries
{
    public class AuthenticationQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAuthenticationService> _authenticationService;

        public AuthenticationQueryHandlerTests()
        {
            _authenticationService = RepositoryMocks.GetAuthenticationService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_WithValidCredentials_ReturnsAuthenticationVm()
        {
            var handler = new AuthenticationQueryHandler(_authenticationService.Object, _mapper);

            var authenticationQuery = new AuthenticationQuery()
            {
                Password = "password",
                Email = "email@gmail.com"
            };

            var result = await handler.Handle(authenticationQuery, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<AuthenticationVm>(result);
        }
    }
}

