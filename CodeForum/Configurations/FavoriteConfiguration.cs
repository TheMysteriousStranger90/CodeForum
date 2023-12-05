using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasOne(favorite => favorite.User)
            .WithMany(user => user.Favorites)
            .HasForeignKey(favorite => favorite.UserId);

        builder.HasOne(favorite => favorite.Topic)
            .WithMany(topic => topic.Favorites)
            .HasForeignKey(favorite => favorite.TopicId);
    }
}