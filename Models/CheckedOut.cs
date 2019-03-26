using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Lizst.Models
{
    public class CheckedOut
    {
        public int MusicianId { get; set; }
        public int PartId { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime DateDue { get; set; }
    }

    public class CheckedOutDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<CheckedOut> CheckedOut { get; set; }
    }
}
