using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace course.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalService> AdditionalServices { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=PostgresConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("PaymentMethod", new[] { "card", "cash", "SBP" });

        modelBuilder.Entity<AdditionalService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("additional_services_pkey");

            entity.ToTable("additional_services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cars_pkey");

            entity.ToTable("cars");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand).HasColumnName("brand");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.User).WithMany(p => p.Cars)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("cars_user_id_fkey");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("news_pkey");

            entity.ToTable("news");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Driver).HasColumnName("driver");
            entity.Property(e => e.EndPoint).HasColumnName("end_point");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.RateId).HasColumnName("rateId");
            entity.Property(e => e.StartingPoint).HasColumnName("starting_point");
            entity.Property(e => e.Tips).HasColumnName("tips");

            entity.HasOne(d => d.Rate).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RateId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("orders_rateId_fkey");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ranks_pkey");

            entity.ToTable("ranks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Position)
                .HasDefaultValueSql("'Пользователь'::text")
                .HasColumnName("position");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rate_pkey");

            entity.ToTable("rate");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllOrders)
                .HasDefaultValue(0)
                .HasColumnName("all_orders");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.GoodFeedBack)
                .HasDefaultValue(0)
                .HasColumnName("good_feed_back");
            entity.Property(e => e.Income)
                .HasDefaultValue(0)
                .HasColumnName("income");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.RankId).HasColumnName("rankId");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("update_at");

            entity.HasOne(d => d.Rank).WithMany(p => p.Users)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_rankId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
