using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Contracts.Services;
using GmailClient.Domain.Entities;
using MediatR;

namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListQueryHandler : IRequestHandler<GetMessagesListQuery, List<GetMessagesListVm>>
    {
        private readonly IGmailService _gmailService;
        private readonly IAsyncRepository<UserGmailToken> _userGmailTokenRepository;
        private readonly IMapper _mapper;
        private readonly IAccessTokenManager _accessTokenManager;
        public GetMessagesListQueryHandler(IGmailService gmailService, IMapper mapper, IAsyncRepository<UserGmailToken> userGmailTokenRepository, IAccessTokenManager accessTokenManager)
        {
            _gmailService = gmailService;
            _mapper = mapper;
            _userGmailTokenRepository = userGmailTokenRepository;
            _accessTokenManager = accessTokenManager;
        }
        public async Task<List<GetMessagesListVm>> Handle(GetMessagesListQuery request, CancellationToken cancellationToken)
        {
            string accessToken = await _accessTokenManager.GetValidAccessTokenAsync(request.UserId);

            var messages = await _gmailService.GetAllMessagesAsync(accessToken);
            return _mapper.Map<List<GetMessagesListVm>>(messages);
        }
    }
}
