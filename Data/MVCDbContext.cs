using FinalTodoApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinalTodoApp.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
