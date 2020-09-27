using Microsoft.EntityFrameworkCore;

using prometheus.model.securitas;

namespace prometheus.data.securitas
{
    public class SecuritasDbContext : DbContext
    {
        public SecuritasDbContext(DbContextOptions<SecuritasDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users","securitas");
            builder.Entity<Role>().ToTable("Roles","securitas");
        }

    }
}