using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using CodeForum.Repositories;
using CodeForum.Services;
using CodeForum.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultLocalDBConnection"));
        });

        services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz";
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>().AddRoles<IdentityRole>();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en-US");
        });
        
        services.AddScoped<IGenericRepository<Topic>, TopicRepository>();
        services.AddScoped<IGenericRepository<Post>, PostRepository>();
        services.AddScoped<IGenericRepository<Rating>, RatingRepository>();
        services.AddScoped<IGenericRepository<Report>, ReportRepository>();
        services.AddScoped<IGenericRepository<Tag>, TagRepository>();
        services.AddScoped<IGenericRepository<TopicTag>, TopicTagRepository>();
        services.AddScoped<IGenericRepository<Category>, CategoryRepository>();
        services.AddScoped<IGenericRepository<Favorite>, FavoriteRepository>();
        services.AddScoped<IGenericRepository<Notification>, NotificationRepository>();
        services.AddScoped<IGenericRepository<LikeDislike>, LikeDislikeRepository>();

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITopicTagRepository, TopicTagRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ILikeDislikeRepository, LikeDislikeRepository>();
        
        services.AddScoped<IFileUploadService, FileUploadService>();
        
        return services;
    }
}