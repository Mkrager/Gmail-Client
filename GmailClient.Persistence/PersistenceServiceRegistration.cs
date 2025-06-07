using GmailClient.Application.Contracts.Persistance;
using GmailClient.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GmailClient.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this
            IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GmailClientDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString
            ("GmailClientConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserGmailTokenRepository, UserGmailTokenRepository>();

            return services;
        }
    }
}
