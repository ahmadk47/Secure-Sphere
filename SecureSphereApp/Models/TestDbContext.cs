using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SecureSphereApp.Models;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Camera> Cameras { get; set; }

    public virtual DbSet<Client> clients { get; set; }

    public virtual DbSet<Detection> Detections { get; set; }

    public virtual DbSet<SystemLog> SystemLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AHMAD\\SQLEXPRESS ;Database=TestDB ;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("Branch");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.ClientId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("client_ID");

            entity.HasOne(d => d.Client).WithMany(p => p.Branches)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_Clients");
        });

        modelBuilder.Entity<Camera>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.BranchId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("branch_ID");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(15)
                .HasColumnName("ip_address");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("status");

            entity.HasOne(d => d.Branch).WithMany(p => p.Cameras)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cameras_Branch");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Detection>(entity =>
        {
            entity.ToTable("Detection");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.CameraId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("camera_ID");
            entity.Property(e => e.Confidence)
                .HasColumnType("numeric(3, 2)")
                .HasColumnName("confidence");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .HasColumnName("reason");
            entity.Property(e => e.Status)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("status");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("user_ID");
            entity.Property(e => e.WeaponType).HasColumnName("weapon_type");

            entity.HasOne(d => d.Camera).WithMany(p => p.Detections)
                .HasForeignKey(d => d.CameraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detection_Cameras");

            entity.HasOne(d => d.User).WithMany(p => p.Detections)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detection_User");
        });

        modelBuilder.Entity<SystemLog>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Details)
                .HasMaxLength(50)
                .HasColumnName("details");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(15)
                .HasColumnName("IP_address");
            entity.Property(e => e.UserId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("user_ID");

            entity.HasOne(d => d.User).WithMany(p => p.SystemLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemLogs_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.BranchId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("branch_ID");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");

            entity.HasOne(d => d.Branch).WithMany(p => p.Users)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Branch");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
