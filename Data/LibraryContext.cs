using System;
using static LizstMVC.Models.LibraryModel;
using Microsoft.EntityFrameworkCore;

namespace LizstMVC.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Score> Score { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=;database=;user=;password=");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Composer).IsRequired();
                entity.Property(e => e.Genre).IsRequired();
                entity.Property(e => e.NumberOfParts).IsRequired();
            });


        }
    }
}