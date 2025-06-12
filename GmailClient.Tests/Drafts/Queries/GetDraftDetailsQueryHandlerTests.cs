using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Queries.GetDraftDetails;
using GmailClient.Tests.Mocks;
using Moq;
using AutoMapper;
using GmailClient.Application.Profiles;

namespace GmailClient.Tests.Drafts.Queries
{
    public class GetDraftDetailsQueryHandlerTests
    {
        private readonly Mock<IGmailService> _gmailService;
        private readonly Mock<IAccessTokenManager> _accessTokenManager;
        private readonly IMapper _mapper;

        public GetDraftDetailsQueryHandlerTests()
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
        public async Task GetDraftDetails_ShouldReturnDraftDto()
        {
            var handler = new GetDraftDetailsQueryHandler(_mapper, _accessTokenManager.Object, _gmailService.Object);
            var query = new GetDraftDetailsQuery
            {
                UserId = "user123",
                DraftId = "draft1"
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(query.DraftId, result.DraftId);
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenDraftIdIsEmpty()
        {
            var validator = new GetDraftDetailsQueryValidator();
            var query = new GetDraftDetailsQuery
            {
                UserId = "user123",
                DraftId = ""
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "DraftId");
        }
    }
}
