using Microsoft.AspNetCore.Identity;

namespace FTC2022_MakingAListAndCheckingItTwice.Data
{
    public class UsersRolesService : IUsersRolesService
    {
        public const string ADMIN_ROLE_NAME = "Admin";
        private const string ADMIN_USER_EMAIL = "mccoy@ncc1701.com";
        private const string ADMIN_USER_PWD = "izDrNotC0d3R!";
        private readonly UserManager<TodoListUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersRolesService(UserManager<TodoListUser> userManager
                                    , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async Task EnsureRoles()
        {
            var existingRole = await _roleManager.FindByNameAsync(ADMIN_ROLE_NAME);
            if (existingRole is null)
            {
                var adminRole = new IdentityRole()
                {
                    Name = ADMIN_ROLE_NAME,
                    NormalizedName = ADMIN_ROLE_NAME.ToUpper()
                };
                await _roleManager.CreateAsync(adminRole);
            }
        }

        private async Task EnsureUsers()
        {
            var existingAdminUser = await _userManager.FindByEmailAsync(ADMIN_USER_EMAIL);
            if (existingAdminUser is null)
            {
                var adminUser = new TodoListUser()
                {
                    Email = ADMIN_USER_EMAIL,
                    EmailConfirmed = true,
                    UserName = ADMIN_USER_EMAIL,
                    NormalizedEmail = ADMIN_USER_EMAIL.ToUpper(),
                    NormalizedUserName = ADMIN_USER_EMAIL.ToUpper(),
                    LockoutEnabled = false
                };

                await _userManager.CreateAsync(adminUser, ADMIN_USER_PWD);
            }
        }

        public async Task EnsureUsersAndRoles()
        {
            //ensure roles
            await EnsureRoles();

            //ensure users
            await EnsureUsers();

            var existingAdminUser = await _userManager.FindByEmailAsync(ADMIN_USER_EMAIL);
            var existingRole = await _roleManager.FindByNameAsync(ADMIN_ROLE_NAME);
            if (existingAdminUser is null || existingRole is null)
            {
                throw new InvalidOperationException("Cannot add  null user/role combination");
            }

            var userRoles = await _userManager.GetRolesAsync(existingAdminUser);
            var existingUserAdminRole = userRoles.SingleOrDefault(x => x.Equals(ADMIN_ROLE_NAME));

            if (existingUserAdminRole is null)
            {
                await _userManager.AddToRoleAsync(existingAdminUser, ADMIN_ROLE_NAME);
            }
        }
    }
}