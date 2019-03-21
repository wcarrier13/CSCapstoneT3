using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using System.Configuration;
using Lizst.Models;

namespace Lizst.Models
{
    public class LizstContext : DbContext
    {
        public LizstContext (DbContextOptions<LizstContext> options)
            : base(options)
        {
        }

        //Configure EnsemblePlayers to use the pair ensembleid and musicianid
        //as primary key, rather than using either on their own.
        //Allows a many to many relationship.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnsemblePlayers>()
                .HasKey(ep => new { ep.EnsembleId, ep.MusicianId });

            //Uses ScoreId as the primary key, one to many
            //This might be wrong
            modelBuilder.Entity<ScorePieces>()
                .HasKey(sp => new { sp.ScoreId });
        }

        

        public DbSet<Score> Score { get; set; }
        public DbSet<Ensemble> Ensemble { get; set; }
        public DbSet<EnsemblePlayers> EnsemblePlayers { get; set; }
        public DbSet<Musician> Musician { get; set; }
        public DbSet<Piece> Piece { get; set; }
        public DbSet<ScorePieces> ScorePieces { get; set; }
    }
}
