using RestaurantReservation.Core.Events;
using RestaurantReservation.Core.Mongo.Data;
using RestaurantReservation.Identity.Contracts;

namespace RestaurantReservation.Identity.Data.Seeders;

public class RoleAndUserSeeder(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IEventDispatcher eventDispatcher) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        await SeedRoles();
        await SeedUsers();
    }

    private async Task SeedRoles()
    {
        if (await roleManager.RoleExistsAsync(Constants.Role.Admin) == false)
            await roleManager.CreateAsync(new Role { Name = Constants.Role.Admin });

        if (await roleManager.RoleExistsAsync(Constants.Role.User) == false)
            await roleManager.CreateAsync(new Role { Name = Constants.Role.User });
    }

    private async Task SeedUsers()
    {
        if (await userManager.FindByEmailAsync(InitialData.Users.First().Email!) == null)
        {
            var result = await userManager.CreateAsync(InitialData.Users.First(), "Admin@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(InitialData.Users.First(), Constants.Role.Admin);
                await eventDispatcher.SendAsync(
                    new UserCreated(
                        InitialData.Users.First().Id,
                        InitialData.Users.First().FirstName + " " + InitialData.Users.First().LastName));
            }
        }

        if (await userManager.FindByEmailAsync(InitialData.Users.Last().Email!) == null)
        {
            var result = await userManager.CreateAsync(InitialData.Users.Last(), "User@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(InitialData.Users.Last(), Constants.Role.User);
                await eventDispatcher.SendAsync(
                    new UserCreated(
                        InitialData.Users.Last().Id,
                        InitialData.Users.Last().FirstName + " " + InitialData.Users.Last().LastName));
            }
        }
    }
}