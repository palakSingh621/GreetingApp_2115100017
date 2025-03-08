using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Context
{
    public class GreetingAppContext: DbContext
    {
        public GreetingAppContext(DbContextOptions<GreetingAppContext> options) : base(options) { }

        public DbSet<Entity.GreetingMessageEntity> GreetingMessages { get; set; }
        public DbSet<Entity.UserEntity> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity.UserEntity>().ToTable("Users");  // Explicitly mapping
        }
    }
}
