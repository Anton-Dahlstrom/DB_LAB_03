using System;
using System.Collections.Generic;
using DB_LAB_03.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_LAB_03;

public partial class Lab02Context : DbContext
{
    public Lab02Context()
    {
    }

    public Lab02Context(DbContextOptions<Lab02Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("LAB_02_connection_string"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC0771980DE8");

            entity.Property(e => e.CourseName)
                .HasMaxLength(128)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC070474C646");

            entity.Property(e => e.FirstName)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grades__3214EC0739B3152A");

            entity.Property(e => e.Grade1).HasColumnName("Grade");
            entity.Property(e => e.GradeDate).HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.Grades)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseGrade");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentGrade");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherGrade");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07567F9EA2");

            entity.HasIndex(e => e.SocialSecurity, "UQ__Students__77F05CB444172205").IsUnique();

            entity.Property(e => e.Class)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.SocialSecurity)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
