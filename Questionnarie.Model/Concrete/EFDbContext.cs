using Tasks.Model.Entities;
using System.Data.Entity;

namespace Tasks.Model.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
    }
}
