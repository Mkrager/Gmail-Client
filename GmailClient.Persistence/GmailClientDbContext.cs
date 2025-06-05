using GmailClient.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GmailClient.Persistence
{
    public class GmailClientDbContext : DbContext
    {
        public GmailClientDbContext(DbContextOptions<GmailClientDbContext> options)
            : base(options)
        {
            

        }

        public DbSet<UserGmailToken> UserGmailTokens { get; set; }
    }
}
