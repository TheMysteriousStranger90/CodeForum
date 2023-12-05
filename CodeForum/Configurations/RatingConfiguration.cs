using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasOne(rating => rating.User)
            .WithMany(user => user.Ratings)
            .HasForeignKey(rating => rating.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rating => rating.Topic)
            .WithMany(topic => topic.Ratings)
            .HasForeignKey(rating => rating.TopicId);
    }
}