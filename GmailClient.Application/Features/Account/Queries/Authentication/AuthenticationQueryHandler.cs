using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.DTOs;
using MediatR;

namespace GmailClient.Application.Features.Account.Queries.Authentication
{
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, AuthenticationVm>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationQueryHandler(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<AuthenticationVm> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
        {
            var authenticationRequest = _mapper.Map<AuthenticationRequest>(request);

            var authentication = await _authenticationService.AuthenticateAsync(authenticationRequest);

            return _mapper.Map<AuthenticationVm>(authentication);
        }
    }
}
