using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using MediatR;

namespace GmailClient.Application.Features.Drafts.Queries.GetDraftDetails
{
    public class GetDraftDetailsQueryHandler : IRequestHandler<GetDraftDetailsQuery, GetDraftDetailsVm>
    {
        private readonly IMapper _mapper;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IGmailService _gmailService;

        public GetDraftDetailsQueryHandler(IMapper mapper, IAccessTokenManager accessTokenManager, IGmailService gmailService)
        {
            _accessTokenManager = accessTokenManager;
            _mapper = mapper;
            _gmailService = gmailService;
        }
        public async Task<GetDraftDetailsVm> Handle(GetDraftDetailsQuery request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            var draft = await _gmailService.GetDraftByIdAsync(accessToken, request.DraftId);

            return _mapper.Map<GetDraftDetailsVm>(draft);
        }
    }
}
