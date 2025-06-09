using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.Features.User.Queries;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.User.Queries
{
    public class GetUserDetailsQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserService> _userService;
        public GetUserDetailsQueryHandlerTests()
        {
            _userService = RepositoryMocks.GetUserService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GetUserDetails_ReturnsUserDetailsVm()
        {
            var handler = new GetUserDetailsQueryHandler(_userService.Object, _mapper);

            var result = await handler.Handle(new GetUserDetailsQuery() { Id = "52543534534" }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<UserDetailsVm>(result);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenUserIdEmpty()
        {
            var validator = new GetUserDetailsQueryValidator();
            var query = new GetUserDetailsQuery
            {
                Id = "",
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Id");
        }
    }
}
