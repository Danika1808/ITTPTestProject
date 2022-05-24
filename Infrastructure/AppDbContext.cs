using Domain;
using Domain.Revoke;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public AppDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IRevoke).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddRevokeQueryFilter();
                }
            }
        }

        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    }

    public enum Order : int
    {
        acs = 0,
        desc = 1
    }
}
