using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lizst.Models
{
    //Stores many to one relationship between pieces and score.
    //Check to see if I can enforce that this is many to one
    public class ScorePieces
    {
        public int ScoreId { get; set; }
        public int PieceId { get; set; }
    }
}
