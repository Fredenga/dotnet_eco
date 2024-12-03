using AuthApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infrastructure.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options): DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}
