﻿using System;
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

        public void AddEnsemble(Ensemble ensemble)
        {
            //Ensembles.Add(ensemble);
        }
    }
}
