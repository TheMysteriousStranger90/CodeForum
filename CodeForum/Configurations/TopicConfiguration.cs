using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class TopicTagConfiguration : IEntityTypeConfiguration<TopicTag>
{
    public void Configure(EntityTypeBuilder<TopicTag> builder)
    {
        builder.HasKey(tt => new { tt.TopicId, tt.TagId });

        builder.HasOne(tt => tt.Topic)
            .WithMany(topic => topic.TopicTags)
            .HasForeignKey(tt => tt.TopicId);

        builder.HasOne(tt => tt.Tag)
            .WithMany(tag => tag.TopicTags)
            .HasForeignKey(tt => tt.TagId);
    }
}