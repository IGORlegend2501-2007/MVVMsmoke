using MVVMsmoke.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMsmoke
{
    class DbSettings : DbContext
    {
        public DbSettings() {   }
        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}
