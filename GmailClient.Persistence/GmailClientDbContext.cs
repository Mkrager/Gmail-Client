using GmailClient.Application.Contracts;
using GmailClient.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GmailClient.Persistence
{
    public class GmailClientDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public GmailClientDbContext(DbContextOptions<GmailClientDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<UserGmailToken> UserGmailTokens { get; set; }
    }
}
