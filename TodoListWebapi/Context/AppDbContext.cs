using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoListWebapi.Model;

namespace TodoListWebapi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }

    }
}
