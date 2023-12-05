using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(user => user.Topics)
            .WithOne(topic => topic.User)
            .HasForeignKey(topic => topic.UserId);

        builder.HasMany(user => user.Posts)
            .WithOne(post => post.User)
            .HasForeignKey(post => post.UserId);

        builder.HasMany(user => user.Notifications)
            .WithOne(notification => notification.User)
            .HasForeignKey(notification => notification.UserId);

        builder.HasMany(user => user.Reports)
            .WithOne(report => report.User)
            .HasForeignKey(report => report.UserId);
    }
}