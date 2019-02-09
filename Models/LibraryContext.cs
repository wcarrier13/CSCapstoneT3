using System;
using LizstMVC.Models;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace LizstMVC.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Score> Score { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=ec2-3-16-188-153.us-east-2.compute.amazonaws.com;database=Lizst;user=lizst;password=BirdCabinet");
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