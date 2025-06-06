using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using MediatR;

namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListQueryHandler : IRequestHandler<GetMessagesListQuery, List<GetMessagesListVm>>
    {
        private readonly IGmailService _gmailService;
        private readonly IMapper _mapper;
        public GetMessagesListQueryHandler(IGmailService gmailService, IMapper mapper)
        {
            _gmailService = gmailService;
            _mapper = mapper;
        }
        public async Task<List<GetMessagesListVm>> Handle(GetMessagesListQuery request, CancellationToken cancellationToken)
        {
            var messages = await _gmailService.GetAllMessagesAsync(request.accessToken);
            return _mapper.Map<List<GetMessagesListVm>>(messages);
        }
    }
}
