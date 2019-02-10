using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using System.Configuration;

namespace Lizst.Models
{
    public class LizstContext : DbContext
    {
        public LizstContext (DbContextOptions<LizstContext> options)
            : base(options)
        {
        }

        public DbSet<Lizst.Models.Test> Test { get; set; }
        public DbSet<Lizst.Models.Score> Score { get; set; }
        public DbSet<Lizst.Models.Ensemble> Ensemble { get; set; }
    }
}
