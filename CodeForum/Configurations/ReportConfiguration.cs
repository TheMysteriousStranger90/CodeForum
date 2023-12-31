﻿using CodeForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeForum.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasOne(report => report.User)
            .WithMany(user => user.Reports)
            .HasForeignKey(report => report.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(report => report.Topic)
            .WithMany()
            .HasForeignKey(report => report.TopicId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(report => report.Post)
            .WithMany()
            .HasForeignKey(report => report.PostId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}