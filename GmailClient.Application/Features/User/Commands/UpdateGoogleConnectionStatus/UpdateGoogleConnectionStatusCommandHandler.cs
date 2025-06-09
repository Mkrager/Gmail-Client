using GmailClient.Application.Contracts.Identity;
using MediatR;

namespace GmailClient.Application.Features.User.Commands.UpdateGoogleConnectionStatus
{
    public class UpdateGoogleConnectionStatusCommandHandler : IRequestHandler<UpdateGoogleConnectionStatusCommand>
    {
        private readonly IUserService _userService;

        public UpdateGoogleConnectionStatusCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Unit> Handle(UpdateGoogleConnectionStatusCommand request, CancellationToken cancellationToken)
        {
            await _userService.SetGoogleConnectedAsync(request.UserId, request.IsConnected);
            return Unit.Value;
        }
    }
}
