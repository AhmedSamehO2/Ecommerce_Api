using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
         if(userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Sameh",
                    Email = "Ahmed@gmail.com",
                    UserName = "Ahmed",
                    PhoneNumber = "01140727984"
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }

        }
    }
}
