using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using MediatR;

namespace GmailClient.Application.Features.User.Queries
{
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsVm>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserDetailsQueryHandler(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<UserDetailsVm> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var userDetails = await _userService.GetUserDetails(request.Id);
            return _mapper.Map<UserDetailsVm>(userDetails);
        }
    }
}
