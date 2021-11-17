using BoggleModel.DataAccess.Entities;
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
        public DbSet<UserAccountEntity> UserAccounts { get; set; }
        
        public DbSet<PlayerEntity> Players { get; set; }

        public DbSet<PerformanceRecordEntity> PerformanceRecords { get; set; }

        public DbSet<FriendRequestEntity> FriendRequests { get; set; }
    }
}
