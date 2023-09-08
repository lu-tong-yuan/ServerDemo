using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace serverDemo.Models;

public partial class ServerDBContext : DbContext
{
    public ServerDBContext(DbContextOptions<ServerDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Course { get; set; }

    public virtual DbSet<CourseDetail> CourseDetail { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseID).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.InstructorNavigation).WithMany(p => p.Course)
                .HasForeignKey(d => d.Instructor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_User");
        });

        modelBuilder.Entity<CourseDetail>(entity =>
        {
            entity.HasKey(e => new { e.CourseID, e.UserID });

            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseDetail)
                .HasForeignKey(d => d.CourseID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseDetail_Course");

            entity.HasOne(d => d.User).WithMany(p => p.CourseDetail)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseDetail_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
