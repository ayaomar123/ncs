using Microsoft.EntityFrameworkCore;
using NCS.Domain.Entities;

namespace NCS.Infrastructure.Persistence;

public class NcsDbContext(DbContextOptions<NcsDbContext> options) : DbContext(options)
{
    public DbSet<Appeal> Appeals => Set<Appeal>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<DonationRequest> DonationRequests => Set<DonationRequest>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NcsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
