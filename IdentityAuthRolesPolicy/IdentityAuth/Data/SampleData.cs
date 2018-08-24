using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace IdentityAuth.Data
{
    public class SampleData
    {
        public static async Task InitializeData(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("SampleData");

            using (var serviceScope = services.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
                if (!env.IsDevelopment()) return;

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                // Create our roles
                var adminTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Admin" });
                var powerUserTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Power Users" });
                Task.WaitAll(adminTask, powerUserTask);
                logger.LogInformation("==> Added Admin and Power Users roles");

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                // Create our default user
                var user = new ApplicationUser
                {
                    Email = "info@infocodify.com",
                    UserName = "info@infocodify.com"
                };
                await userManager.CreateAsync(user, "Infocodify2018!");
                logger.LogInformation($"==> Create user info@infocodify.com with password Infocodify2018!");

                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Country, "Canada"));
            }

        }

        internal static void InitializeData(IServiceProvider applicationServices, object loggerFactory)
        {
            throw new NotImplementedException();
        }
    }
}
