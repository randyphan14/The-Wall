using Microsoft.EntityFrameworkCore;

namespace TheWall.Models
{
    public class YourContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public YourContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}

        public DbSet<Message> Messages {get;set;}

        public DbSet<Comment> Comments {get;set;}
    }
}