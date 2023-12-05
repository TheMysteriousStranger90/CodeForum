using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasOne(topic => topic.User)
            .WithMany(user => user.Topics)
            .HasForeignKey(topic => topic.UserId);

        builder.HasOne(topic => topic.Category)
            .WithMany(category => category.Topics)
            .HasForeignKey(topic => topic.CategoryId);

        builder.HasMany(topic => topic.Posts)
            .WithOne(post => post.Topic)
            .HasForeignKey(post => post.TopicId);

        builder.HasMany(topic => topic.Ratings)
            .WithOne(rating => rating.Topic)
            .HasForeignKey(rating => rating.TopicId);

        builder.HasMany(topic => topic.Favorites)
            .WithOne(favorite => favorite.Topic)
            .HasForeignKey(favorite => favorite.TopicId);

        builder.HasMany(topic => topic.TopicTags)
            .WithOne(tt => tt.Topic)
            .HasForeignKey(tt => tt.TopicId);
    }
}