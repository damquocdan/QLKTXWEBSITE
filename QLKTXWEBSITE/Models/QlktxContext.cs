using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLKTXWEBSITE.Models;

public partial class QlktxContext : DbContext
{
    public QlktxContext()
    {
    }

    public QlktxContext(DbContextOptions<QlktxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<BedOfRoom> BedOfRooms { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Dh> Dhs { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Occupancy> Occupancies { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DAMQUOCDAN;Database=QLKTX;uid=sa;pwd=1234;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E8CC6F1E2B");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<BedOfRoom>(entity =>
        {
            entity.HasKey(e => e.BedId).HasName("PK__BedOfRoo__A8A710606D8EE4BB");

            entity.ToTable("BedOfRoom");

            entity.Property(e => e.BedId).HasColumnName("BedID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Room).WithMany(p => p.BedOfRooms)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomID_Bed");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD5881B029");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Dh>(entity =>
        {
            entity.HasKey(e => e.Dhid).HasName("PK__DH__2971B07DD86B7288");

            entity.ToTable("DH");

            entity.Property(e => e.Dhid).HasColumnName("DHID");
            entity.Property(e => e.Dhcode)
                .HasMaxLength(10)
                .HasColumnName("DHCode");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDD3759B4971");

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.PublishedDate).HasColumnType("date");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Occupancy>(entity =>
        {
            entity.HasKey(e => e.OccupancyId).HasName("PK__Occupanc__E8FD04F66F1FEB3D");

            entity.ToTable("Occupancy");

            entity.Property(e => e.OccupancyId).HasColumnName("OccupancyID");
            entity.Property(e => e.ExpirationDate).HasColumnType("date");
            entity.Property(e => e.RenewalDate).HasColumnType("date");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Room).WithMany(p => p.Occupancies)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomID_Occupancy");

            entity.HasOne(d => d.Student).WithMany(p => p.Occupancies)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentID_Occupancy");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__32863919D01375EE");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Building).HasMaxLength(10);
            entity.Property(e => e.Mowroom)
                .HasMaxLength(10)
                .HasColumnName("MOWRoom");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EA308F48E6");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Room).WithMany(p => p.Services)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomID_Service");

            entity.HasOne(d => d.Student).WithMany(p => p.Services)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentID_Service");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79AC91B3D3");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.AdmissionConfirmation).HasMaxLength(100);
            entity.Property(e => e.BedId).HasColumnName("BedID");
            entity.Property(e => e.Class).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Dhid).HasColumnName("DHID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ParentPhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StudentCode).HasMaxLength(20);

            entity.HasOne(d => d.Bed).WithMany(p => p.Students)
                .HasForeignKey(d => d.BedId)
                .HasConstraintName("FK_BedID_Student");

            entity.HasOne(d => d.Department).WithMany(p => p.Students)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_DepartmentID");

            entity.HasOne(d => d.Dh).WithMany(p => p.Students)
                .HasForeignKey(d => d.Dhid)
                .HasConstraintName("FK_DHID");

            entity.HasOne(d => d.Room).WithMany(p => p.Students)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomID_Student");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B3DB72CEC");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TransactionCode).HasMaxLength(20);
            entity.Property(e => e.TransactionDate).HasColumnType("date");

            entity.HasOne(d => d.Student).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentID_Transaction");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    internal object GetById(int bedId)
    {
        throw new NotImplementedException();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
