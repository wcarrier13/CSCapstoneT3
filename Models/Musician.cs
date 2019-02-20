using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lizst.Models
{
    public class Musician
    {
        public int MusicianId { get; set; }
        public int StudentId { get; set; }
        public string MusicianName { get; set; }
        public string Part { get; set; }
        public string Email { get; set; }
        public ICollection<EnsemblePlayers> Ensembles { get; } = new List<EnsemblePlayers>();

        // Link the musician model to an ensemble.
        // Musicians and ensembles may have a many to many relationship.
        public void AddEnsemble(Ensemble ensemble, LizstContext context)
        {
            ensemble.AddPlayer(this, context);
            //Ensembles.Add(ensemble);
        }
    }
}
