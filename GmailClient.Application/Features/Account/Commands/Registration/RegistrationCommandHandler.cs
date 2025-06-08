using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.DTOs;
using MediatR;

namespace GmailClient.Application.Features.Account.Commands.Registration
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public RegistrationCommandHandler(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<string> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var registrationRequest = _mapper.Map<RegistrationRequest>(request);

            var register = await _authenticationService.RegisterAsync(registrationRequest);

            return register;
        }
    }
}
