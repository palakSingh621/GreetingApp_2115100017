using ModelLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        [Required]
        public DbSet<UserModel> Users { get; set; }
    }
}
