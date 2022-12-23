using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using TodoListModels;

namespace TodoListData
{
    public class TodoListDataContext : DbContext
    {

        private static IConfigurationRoot _configuration;

        public DbSet<TodoListItem> ToDoItems { get; set; }

        public TodoListDataContext()
        {
            //leave this here for scaffolding, etc
        }

        public TodoListDataContext(DbContextOptions options)
            : base(options)
        {
            //purposefully blank
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                _configuration = builder.Build();
                var cnstr = _configuration.GetConnectionString("TodoListDataDbConnection");
                optionsBuilder.UseSqlServer(cnstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
        
}