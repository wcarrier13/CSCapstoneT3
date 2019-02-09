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
        public int ID { get; set; }
        public string Instrument { get; set; }
        public string Number { get; set; }
        public DateTime DateCheckedOut { get; set; }
        public DateTime DueDate { get; set; }
        public string EnsembleMember { get; set; }

    }


    public class PieceDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Piece> piece { get; set; }
    }

}