using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EMI.Domain.Entities;

namespace EMI.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Email> Emails { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Customer>(entity =>

            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Loan>().HasKey(l => l.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

            modelBuilder.Entity<Email>(entity =>
            {

                entity.ToTable("Emails");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Body).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Subject).HasMaxLength(500);
            });
        }
    }
}