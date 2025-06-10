using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Infrastructure.Gmail;
using GmailClient.Infrastructure.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GmailClient.Infrastructure.GoogleOAuth;

namespace GmailClient.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGmailService, GmailService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IGoogleOAuthStateService, GoogleOAuthStateService>();
            services.AddTransient<ITokenEncryptionService, TokenEncryptionService>();

            services.AddHttpClient<IGoogleOAuthService, GoogleOAuthService>();

            services.AddMemoryCache();
            services.AddDataProtection();

            return services;
        }
    }
}
