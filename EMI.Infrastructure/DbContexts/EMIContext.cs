using EMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMI.Infrastructure.DbContexts
{
    public class EMIContext : DbContext
    {
        public EMIContext(DbContextOptions<EMIContext> options) : base(options) 
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>()
                .HasAlternateKey(e => e.Email)
                .HasName("AK_Tenant_Email");

        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreationDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        #region DB Sets
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Email> Emails{ get; set; }






        #endregion
    }
}
