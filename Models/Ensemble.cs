using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lizst.Models
{
    public class Ensemble
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Conductor { get; set; }
        public int Year { get; set; }
        public virtual ICollection<EnsembleMember> EnsembleMembers { get; set; }
    }
}
