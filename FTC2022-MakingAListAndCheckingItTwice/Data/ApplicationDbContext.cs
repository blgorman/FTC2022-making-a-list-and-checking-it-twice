using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FTC2022_MakingAListAndCheckingItTwice.Data
{
    public class ApplicationDbContext : IdentityDbContext<TodoListUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}