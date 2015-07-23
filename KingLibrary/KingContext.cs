using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class KingContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
    }
}
