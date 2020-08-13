using Microsoft.EntityFrameworkCore;

namespace FinalExam.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        // this is the variable we will use to connect to the MySQL table, Dishes
        public DbSet<User> Users { get; set; }
        public DbSet<ActivityCenter> Activities { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}