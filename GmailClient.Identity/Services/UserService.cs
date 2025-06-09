using GmailClient.Application.Contracts.Identity;
using GmailClient.Application.DTOs;
using GmailClient.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace GmailClient.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserDetailsResponse> GetUserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found.");
            }

            UserDetailsResponse userDetailsResponse = new UserDetailsResponse()
            {
                Email = user.Email,
                UserName = user.UserName,
                IsGoogleConnected = user.IsGoogleConnected,
            };

            return userDetailsResponse;
        }

        public async Task SetGoogleConnectedAsync(string userId, bool isConnected)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            user.IsGoogleConnected = isConnected;
            await _userManager.UpdateAsync(user);
        }
    }
}
