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
        public string Part { get; set; }
        public string Condition { get; set; }
        public int ScoreId { get; set; }

    }


    public class PieceDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Piece> piece { get; set; }
    }

}