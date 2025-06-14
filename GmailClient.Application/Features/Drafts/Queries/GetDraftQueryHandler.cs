using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using GmailClient.Application.Features.Drafts.Queries.GetDraftDetails;
using GmailClient.Application.Features.Drafts.Queries.GetDraftsList;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Queries
{
    public class GetDraftQueryHandler :
        IRequestHandler<GetDraftDetailsQuery, GetDraftDetailsVm>,
        IRequestHandler<GetDraftsListQuery, List<GetDraftsListVm>>
    {
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IGmailService _gmailService;
        private readonly IMapper _mapper;

        public GetDraftQueryHandler(IAccessTokenManager accessTokenManager, IGmailService gmailService, IMapper mapper)
        {
            _accessTokenManager = accessTokenManager;
            _gmailService = gmailService;
            _mapper = mapper;
        }

        public async Task<GetDraftDetailsVm> Handle(GetDraftDetailsQuery request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            var draft = await _gmailService.GetDraftByIdAsync(accessToken, request.DraftId);

            return _mapper.Map<GetDraftDetailsVm>(draft);
        }

        public async Task<List<GetDraftsListVm>> Handle(GetDraftsListQuery request, CancellationToken cancellationToken)
        {
            string accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            var result = await _gmailService.GetDraftsAsync(accessToken);

            return _mapper.Map<List<GetDraftsListVm>>(result);
        }
    }
}
