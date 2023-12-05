using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class LikeDislikeConfiguration : IEntityTypeConfiguration<LikeDislike>
{
    public void Configure(EntityTypeBuilder<LikeDislike> builder)
    {
        builder.HasOne(likeDislike => likeDislike.User)
            .WithMany(user => user.LikesDislikes)
            .HasForeignKey(likeDislike => likeDislike.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(likeDislike => likeDislike.Post)
            .WithMany(post => post.LikesDislikes)
            .HasForeignKey(likeDislike => likeDislike.PostId);
    }
}