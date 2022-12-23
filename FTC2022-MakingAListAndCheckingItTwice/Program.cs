using FTC2022_MakingAListAndCheckingItTwice.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListData;

namespace FTC2022_MakingAListAndCheckingItTwice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Application Db Context.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //Add TodoListData Context
            var todoListConnection = builder.Configuration.GetConnectionString("TodoListDataDbConnection") ?? throw new InvalidOperationException("Connection string 'TodoListDataDbConnection' not found.");
            builder.Services.AddDbContext<TodoListDataContext>(options =>
                options.UseSqlServer(todoListConnection));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            /*
            builder.Services.AddAuthentication().AddMicrosoftAccount(options => {
                options.ClientId = builder.Configuration["Authorization:Microsoft:ClientID"];
                options.ClientSecret = builder.Configuration["Authorization:Microsoft:ClientSecret"];
            });
            */

            builder.Services.AddDefaultIdentity<TodoListUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            /***************************************************/
            /* Uncomment below to Enable automatic migrations (use with caution!) */
            /***************************************************/

            
            
            var identityContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                .UseSqlServer(connectionString).Options;
            using (var context = new ApplicationDbContext(identityContextOptions))
            {
                context.Database.Migrate();
            }
            var todoContextOptions = new DbContextOptionsBuilder<TodoListDataContext>()
                                                .UseSqlServer(todoListConnection).Options;
            using (var context = new TodoListDataContext(todoContextOptions))
            {
                context.Database.Migrate();
            }
            
            
            /**************************************************/

            /* UsersRolesService */
            //builder.Services.AddScoped<IUsersRolesService, UsersRolesService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}