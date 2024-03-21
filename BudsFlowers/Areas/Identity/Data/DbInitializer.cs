using Microsoft.AspNetCore.Identity;

namespace BudsFlowers.Areas.Identity.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string userName = "Администратор";
            string phone = "+375298670467";
            string adminEmail = "budsflowers.grodno@gmail.com";
            string password = "budsflowers.grodno.2024";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = userName, PhoneNumber = phone, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
