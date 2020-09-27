using Microsoft.EntityFrameworkCore;

using prometheus.model.gym;
using prometheus.model.securitas;

namespace prometheus.data.gym
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) {}

        public DbSet<Member> Members { get; set; }
        public DbSet<AuthorizedCapacity> AuthorizedCapacities { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<ValidationType> ValidationTypes { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users", "securitas");
            modelBuilder.Entity<Role>().ToTable("Roles", "securitas");
        }


    }
}