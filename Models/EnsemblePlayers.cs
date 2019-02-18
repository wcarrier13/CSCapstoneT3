using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lizst.Models
{
    public class EnsemblePlayers
    {
        public int EnsembleId { get; set; }
        public int MusicianId { get; set; }
    }
}
