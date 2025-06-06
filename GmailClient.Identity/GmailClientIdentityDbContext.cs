using GmailClient.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GmailClient.Identity
{
    public class GmailClientIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public GmailClientIdentityDbContext()
        {

        }

        public GmailClientIdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            
        }
    }
}
