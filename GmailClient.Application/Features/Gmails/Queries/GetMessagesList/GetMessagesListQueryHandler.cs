using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Services;
using MediatR;

namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListQueryHandler : IRequestHandler<GetMessagesListQuery, GetMessagesListVm>
    {
        private readonly IGmailService _gmailService;
        private readonly IMapper _mapper;
        private readonly IAccessTokenManager _accessTokenManager;
        public GetMessagesListQueryHandler(IGmailService gmailService, IMapper mapper, IAccessTokenManager accessTokenManager)
        {
            _gmailService = gmailService;
            _mapper = mapper;
            _accessTokenManager = accessTokenManager;
        }
        public async Task<GetMessagesListVm> Handle(GetMessagesListQuery request, CancellationToken cancellationToken)
        {
            string accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            var messages = await _gmailService.GetAllMessagesAsync(accessToken, request.NextPageToken);
            return _mapper.Map<GetMessagesListVm>(messages);
        }
    }
}
