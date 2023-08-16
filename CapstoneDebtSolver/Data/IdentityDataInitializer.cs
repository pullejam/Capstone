using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneDebtSolver.Data
{
    public class IdentityDataInitializer
    {
        public static void Initialize(UserManager<IdentityUser> userManager,
               RoleManager<IdentityRole> roleManager)
        {

            SetupRoles(roleManager);
            SetupUsers(userManager);
        }

        public static void SetupUsers(UserManager<IdentityUser> userManager)
        {
            var existingUser = userManager.FindByNameAsync("test@here.com").Result;
            if (existingUser == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "test@here.com";
                user.Email = "test@here.com";
                user.PhoneNumberConfirmed = true;
                user.EmailConfirmed = true;
                user.Id = "b90576a6-eea6-4031-8c31-63c2249003e7";
                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;
            }

            existingUser = userManager.FindByNameAsync("admin@admin.com").Result;
            if (existingUser == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                user.PhoneNumberConfirmed = true;
                user.EmailConfirmed = true;
                user.Id = "c81e5ecb-985e-4485-a2c9-7a12173cf5fe";

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        public static void SetupRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleExists = roleManager.RoleExistsAsync("User").Result;
            if (!roleExists)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            roleExists = roleManager.RoleExistsAsync("Admin").Result;
            if (!roleExists)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
