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
        public string Notes { get; set; }
        //public ICollection<Piece> Pieces { get; } = new List<Piece>();
        //public IDictionary<string, Piece> IndexedPieces = new Dictionary<string, Piece>();
    }




    public class ScoreDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Score> scores { get; set; }
    }

}