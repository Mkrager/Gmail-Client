using AutoMapper;
using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.Features.User.Commands.UpdateGoogleConnectionStatus;
using GmailClient.Application.Profiles;
using GmailClient.Tests.Mocks;
using MediatR;
using Moq;

namespace GmailClient.Tests.User.Commands
{
    public class UpdateGoogleConnectionStatusCommandTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserService> _userService;
        public UpdateGoogleConnectionStatusCommandTests()
        {
            _userService = RepositoryMocks.GetUserService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task UpdateGoogleIsGoogleConnectdeStatus_RenrnsTask()
        {
            var handler = new UpdateGoogleConnectionStatusCommandHandler(_userService.Object);

            var result = await handler.Handle(new UpdateGoogleConnectionStatusCommand() 
            { 
                IsConnected = true,
                UserId = "53453453453"
            }, CancellationToken.None);

            Assert.IsType<Unit>(result);
        }
    }
}
