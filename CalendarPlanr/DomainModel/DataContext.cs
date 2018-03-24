using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;

namespace CalendarPlanr.DomainModel
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        { }
        public DbSet<CLogin> CLogins { get; set; }
        public DbSet<CEvent> CEvents { get; set; }
    }
}
