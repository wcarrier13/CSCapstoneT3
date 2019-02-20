using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lizst.Models
{
    //Stores many to many relationship between ensembles and musicians.
    public class EnsemblePlayers
    {
        public int EnsembleId { get; set; }
        public int MusicianId { get; set; }
    }
}
