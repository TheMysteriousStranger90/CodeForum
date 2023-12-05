using CodeForum.Context;
using CodeForum.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeForum.SeedData;

public static class SeedDataInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (!context.Roles.Any())
        {
            await SeedRolesAsync(roleManager);
        }

        if (!context.Users.Any())
        {
            await SeedUsersAsync(userManager, context);
        }
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new List<string> { "Administrator", "User" };
        foreach (var roleName in roles)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        string adminEmail = "lev.myshkin@outlook.com";
        string password = "Myshkin0101";

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            ApplicationUser admin = new ApplicationUser
            {
                FirstName = "Lev",
                LastName = "Myshkin",
                Email = adminEmail,
                UserName = adminEmail,
                DateOfBirth = new DateTime(1980, 1, 1),
                Bio = "Admin user",
                ProfilePicture = "default.jpg"
            };

            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Administrator");
            }
        }

        var users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                FirstName = "User1", LastName = "User1", Email = "user1@example.com", UserName = "user1@example.com",
                DateOfBirth = new DateTime(1990, 1, 1), Bio = "User 1", ProfilePicture = "default.jpg"
            },
            new ApplicationUser
            {
                FirstName = "User2", LastName = "User2", Email = "user2@example.com", UserName = "user2@example.com",
                DateOfBirth = new DateTime(1990, 1, 1), Bio = "User 2", ProfilePicture = "default.jpg"
            },
        };

        foreach (var user in users)
        {
            if (await userManager.FindByNameAsync(user.Email) == null)
            {
                IdentityResult result = await userManager.CreateAsync(user, "Password123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        foreach (var user in users)
        {
            await SeedTopicsAndPostsForUser(user, context);
        }
    }

    private static async Task SeedTopicsAndPostsForUser(ApplicationUser user, ApplicationDbContext context)
    {
        var category = new Category { Name = "C# Programming Language" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var topic = new Topic
        {
            Title = "C# New Features",
            Content = "Discussion about C# programming",
            CreatedAt = DateTime.Now,
            UserId = user.Id,
            CategoryId = category.Id,
            Posts = new List<Post>
            {
                new Post
                {
                    Content = $"Hello, I am {user.FirstName}. I have a question about C#.",
                    CreatedAt = DateTime.Now,
                    UserId = user.Id,
                    Image = "default.jpg"
                },
                new Post
                {
                    Content = $"Hello, I am {user.FirstName}. I have an answer about C#.",
                    CreatedAt = DateTime.Now,
                    UserId = user.Id,
                    Image = "default.jpg"
                }
            }
        };

        await context.Topics.AddAsync(topic);
        await context.SaveChangesAsync();
    }
}