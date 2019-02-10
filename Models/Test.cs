using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Lizst.Models
{
    public class Test
    {
        public int ID { get; set; }

    }

    
    public class TestDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Test> tests { get; set; }
    }
    
}
