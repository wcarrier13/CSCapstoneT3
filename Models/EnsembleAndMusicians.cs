using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lizst.Models
{
    public class EnsembleAndMusicians
    {
        public Ensemble Ensemble;
        public IEnumerable<Musician> Musicians;

        public EnsembleAndMusicians(Ensemble e, IEnumerable<Musician> ms)
        {
            Ensemble = e;
            Musicians = ms;
        }
    }
}
