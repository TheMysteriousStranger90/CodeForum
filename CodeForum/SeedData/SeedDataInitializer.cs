using CodeForum.Context;
using CodeForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            if (await roleManager.FindByIdAsync(roleName) == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role: {result.Errors.First().Description}");
                }
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        var admin = CreateAdminUser();
        await CreateUser(admin, "Myshkin0101", "Administrator", userManager, context);

        var users = CreateUsers();
        foreach (var user in users)
        {
            await CreateUser(user, "Password123", "User", userManager, context);
        }
    }

    private static ApplicationUser CreateAdminUser()
    {
        return new ApplicationUser
        {
            FirstName = "Lev",
            LastName = "Myshkin",
            Email = "lev.myshkin@outlook.com",
            UserName = "lev.myshkin@outlook.com",
            DateOfBirth = new DateTime(1980, 1, 1),
            Bio = "Admin user",
            ProfilePicture = "default.jpg"
        };
    }

    private static List<ApplicationUser> CreateUsers()
    {
        return new List<ApplicationUser>
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
    }

    private static async Task CreateUser(ApplicationUser user, string password, string roleName,
        UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        if (await userManager.FindByNameAsync(user.Email) == null)
        {
            IdentityResult result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
                await SeedTopicsAndPostsForUser(user, context);
            }
            else
            {
                throw new Exception($"Failed to create user: {result.Errors.First().Description}");
            }
        }
    }

    private static async Task SeedTopicsAndPostsForUser(ApplicationUser user, ApplicationDbContext context)
    {
        var categoryName = "C# Programming Language";
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
        if (category == null)
        {
            category = new Category { Name = categoryName };
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }

        var topic = CreateTopicForUser(user, category);

        await context.Topics.AddAsync(topic);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save topic and posts: {ex.Message}");
            throw;
        }
    }

    private static Topic CreateTopicForUser(ApplicationUser user, Category category)
    {
        return new Topic
        {
            Title = "C# New Features",
            Content = "Discussion about C# programming",
            CreatedAt = DateTime.Now,
            UserId = user.Id,
            CategoryId = category.Id,
            Image = "default.jpg", // Ensure 'Image' is never NULL
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
    }
}