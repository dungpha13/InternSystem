﻿using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AmazingTech.InternSystem.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        IConfiguration _configuration;

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DatabaseFacade DatabaseFacade => throw new NotImplementedException();

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<NhomZalo> NhomZalos { get; set; }
        public DbSet<UserNhomZalo> UserNhomZalos { get; set; }
        public DbSet<LichPhongVan> LichPhongVans { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<KiThucTap> KiThucTaps { get; set; }
        public DbSet<TruongHoc> TruongHocs { get; set; }
        public DbSet<InternInfo> InternInfos { get; set; }
        public DbSet<UserDuAn> InternDuAns { get; set; }
        public DbSet<DuAn> DuAns { get; set; }
        public DbSet<CongNgheDuAn> CongNgheDuAns { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"),
                options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRole"));

            modelBuilder.Entity<LichPhongVan>()
                .HasKey(lp => new { lp.IdNguoiDuocPhongVan, lp.IdNguoiPhongVan });

            modelBuilder.Entity<LichPhongVan>()
                .HasOne(lp => lp.NguoiPhongVan)
                .WithMany(u => u.LichPhongVans_PhongVan)
                .HasForeignKey(lp => lp.IdNguoiPhongVan)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LichPhongVan>()
                .HasOne(lp => lp.NguoiDuocPhongVan)
                .WithMany(u => u.LichPhongVans_DuocPhongVan)
                .HasForeignKey(lp => lp.IdNguoiDuocPhongVan)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ThongBao>()
                .HasOne(lp => lp.NguoiGui)
                .WithMany(u => u.ThongBao_NguoiGui)
                .HasForeignKey(lp => lp.IdNguoiGui)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ThongBao>()
                .HasOne(lp => lp.NguoiNhan)
                .WithMany(u => u.ThongBao_NguoiNhan)
                .HasForeignKey(lp => lp.IdNguoiNhan)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(lp => lp.NguoiComment)
                .WithMany(u => u.Comments)
                .HasForeignKey(lp => lp.IdNguoiComment)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(lp => lp.NguoiDuocComment)
                .WithMany(u => u.Comments)
                .HasForeignKey(lp => lp.IdNguoiDuocComment)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserNhomZalo>()
                .HasOne(zl => zl.User)
                .WithMany(u => u.UserNhomZalos)
                .HasForeignKey(zl => zl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserNhomZalo>()
                .HasOne(zl => zl.NhomZalo)
                .WithMany(u => u.UserNhomZalos)
                .HasForeignKey(zl => zl.IdNhomZalo)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDuAn>()
                .HasOne(zl => zl.User)
                .WithMany(u => u.UserDuAns)
                .HasForeignKey(zl => zl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDuAn>()
                .HasOne(zl => zl.User)
                .WithMany(u => u.UserDuAns)
                .HasForeignKey(zl => zl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Auto generate when inserted/updated

            modelBuilder.Entity<Comment>()
                .Property(e => e.CreatedTime)
        .       ValueGeneratedOnAdd();
            modelBuilder.Entity<Comment>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<CongNghe>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<CongNghe>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<CongNgheDuAn>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<CongNgheDuAn>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<DuAn>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<DuAn>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<InternInfo>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<InternInfo>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<KiThucTap>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<KiThucTap>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<LichPhongVan>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<LichPhongVan>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<ViTri>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ViTri>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<UserNhomZalo>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UserNhomZalo>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<UserDuAn>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UserDuAn>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<User>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<TruongHoc>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TruongHoc>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<ThongBao>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ThongBao>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();
            
            modelBuilder.Entity<NhomZalo>()
                .Property(e => e.CreatedTime)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<NhomZalo>()
                .Property(e => e.LastUpdatedTime)
                .ValueGeneratedOnAddOrUpdate();

            base.OnModelCreating(modelBuilder);
        }
    }
}