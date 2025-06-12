using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Queries.GetDraftsList;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using Moq;

namespace GmailClient.Tests.Drafts.Queries
{
    public class GetDraftsListQueryHandlerTests
    {
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;
        private readonly IMapper _mapper;

        public GetDraftsListQueryHandlerTests()
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
        public async Task GetDraftsList_ShouldReturnListOfDrafts()
        {
            var handler = new GetDraftsListQueryHandler(_accessTokenManager.Object, _gmailService.Object, _mapper);
            var query = new GetDraftsListQuery
            {
                UserId = "user123"
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal("draft1", result[0].DraftId);
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenUserIdIsEmpty()
        {
            var validator = new GetDraftsListQueryValdiator();
            var query = new GetDraftsListQuery
            {
                UserId = "" 
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserId");
        }
    }
}
