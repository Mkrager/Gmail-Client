using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Queries.GetDraftsList
{
    public class GetDraftsListQueryHandler : IRequestHandler<GetDraftsListQuery, List<GetDraftsListVm>>
    {
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IGmailService _gmailService;
        private readonly IMapper _mapper;

        public GetDraftsListQueryHandler(IAccessTokenManager accessTokenManager, IGmailService gmailService, IMapper mapper)
        {
            _accessTokenManager = accessTokenManager;
            _gmailService = gmailService;
            _mapper = mapper;
        }
        public async Task<List<GetDraftsListVm>> Handle(GetDraftsListQuery request, CancellationToken cancellationToken)
        {
            string accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            var result = await _gmailService.GetDraftsAsync(accessToken);

            return _mapper.Map<List<GetDraftsListVm>>(result);
        }
    }
}
