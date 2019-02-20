using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Lizst.Models
{
    public class Ensemble
    {
        public int EnsembleId { get; set; }
        public string EnsembleName { get; set; }
        public string Conductor { get; set; }
        public string Year { get; set; }
        public ICollection<EnsemblePlayers> Players { get; } = new List<EnsemblePlayers>();

        //Link ensemble and musician. The two may have a many to many relationship.
        public void AddPlayer(Musician musician, LizstContext context)
        {
            // New database record for the link.
            EnsemblePlayers pair = new EnsemblePlayers
            {
                EnsembleId = EnsembleId,
                MusicianId = musician.MusicianId
            };
            Players.Add(pair);
            context.EnsemblePlayers.Add(pair);
            context.SaveChanges();
        }
    }

    public class EnsembleContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Ensemble> Ensembles { get; set; }
    }
}
