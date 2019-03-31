using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Lizst.Models
{
    public class Score
    {
        

        public int ScoreId { get; set; }
        public string Title { get; set; }
        public string Composer { get; set; }
        public string Genre { get; set; }
        public int NumberOfParts { get; set; }
        public string Publisher { get; set; }
        public string SecondaryClassification { get; set; }
        public string InStock { get; set; }

      
    
            //public ICollection<ScorePieces> Pieces { get; } = new List<ScorePieces>();

        /*public void AddPiece(Piece piece, LizstContext context)
        {
            // New database record for the link.
            ScorePieces pair = new ScorePieces
            {
                ScoreId = ScoreId,
                PieceId = piece.PieceId
            };
            Pieces.Add(pair);
            context.ScorePieces.Add(pair);
            context.SaveChanges();
        }
        */
    }




    public class ScoreDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Score> scores { get; set; }
    }

}