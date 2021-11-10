using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class BoggleContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        
        public DbSet<Player> Players { get; set; }

        public DbSet<PerformanceRecord> PerformanceRecords { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }
    }
}
