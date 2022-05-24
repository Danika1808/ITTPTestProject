using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            string login = configuration["AdminLogin"];
            string password = configuration["AdminPassword"];
            string name = configuration["AdminName"];

            if (await roleManager.FindByNameAsync(Constants.AdminRole) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(Constants.AdminRole));
            }

            var id = Guid.NewGuid();

            if (await userManager.Users.IgnoreQueryFilters().AnyAsync(x => x.Login == login))
            {
                var admin = new ApplicationUser
                {
                    Id = id,
                    Login = login,
                    UserName = login,
                    Name = name,
                    CreatedBy = login,
                    Gender = 1,
                    CreatedOn = DateTime.UtcNow,
                    IsAdmin = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new string[] { Constants.AdminRole });
                }
            }
        }
    }
}
