using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Infrastructure.Gmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GmailClient.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGmailService, GmailService>();

            return services;
        }
    }
}
