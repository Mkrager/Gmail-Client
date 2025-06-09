using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Gmails.Queries.GetMessagesList;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.Gmails.Queries
{
    public class GetMessagesListQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;

        public GetMessagesListQueryHandlerTests()
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
        public async Task GetMessagesList_ReturnsListOfMessages()
        {
            var handler = new GetMessagesListQueryHandler(_gmailService.Object, _mapper, _accessTokenManager.Object);

            var result = await handler.Handle(new GetMessagesListQuery() { UserId = "someUserId" }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }
    }
}
