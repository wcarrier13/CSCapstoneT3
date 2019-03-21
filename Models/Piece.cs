using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Lizst.Models
{
    public class Piece
    {
        public int PieceId { get; set; }
        public int NumberofParts { get; set; }
        public string Instrument { get; set; }
        public int ScoreId { get; set; }
        public string Edition { get; set; }
        public int AggregateRating { get; set; }


        public void AddScore(Score score, LizstContext context)
        {
            score.AddPiece(this, context);
        }
    }

    public class PieceDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Piece> piece { get; set; }
    }

}