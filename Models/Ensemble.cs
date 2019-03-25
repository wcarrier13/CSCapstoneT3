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

    }

    public class EnsembleContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Ensemble> Ensembles { get; set; }
    }
}
