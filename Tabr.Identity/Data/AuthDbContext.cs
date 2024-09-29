using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tabr.Identity.Data.Configurations;
using Tabr.Identity.Entities.User;

namespace Tabr.Identity.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        public AuthDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<string>>(entity =>
                entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity =>
                entity.ToTable(name: "UserClaim"));
            builder.Entity<IdentityUserLogin<string>>(entity =>
                entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity =>
                entity.ToTable(name: "UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity =>
                entity.ToTable(name: "RoleClaim"));
            builder.ApplyConfiguration(new AppUserConfiguration());
            base.OnModelCreating(builder);
        }
    }
}