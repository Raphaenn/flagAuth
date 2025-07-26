using Domain.Entities;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class InfraDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=1234;Database=flags;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserView>(entity =>
        {
            entity.ToTable("users_view");
            entity.HasKey(uv => uv.Id);
            entity.Property(uv => uv.Id).HasColumnName("id");
            entity.Property(uv => uv.Name).HasColumnName("name");
            entity.Property(uv => uv.Email).HasColumnName("email");
            entity.Property(uv => uv.Email).HasColumnName("email");
            entity.Property(uv => uv.Country).HasColumnName("country");
            entity.Property(uv => uv.City).HasColumnName("city");
            entity.Property(uv => uv.Birthdate).HasColumnName("birthdate");
            entity.Property(uv => uv.Sexuality).HasColumnName("sexuality");
            entity.Property(uv => uv.SexualOrientation).HasColumnName("sexual_orientation");
            entity.Property(uv => uv.Password).HasColumnName("password");
            entity.Property(uv => uv.Height).HasColumnName("height");
            entity.Property(uv => uv.Weight).HasColumnName("weight");
            entity.Property(uv => uv.Latitude).HasColumnName("latitude");
            entity.Property(uv => uv.Longitude).HasColumnName("longitude");
            entity.Property(uv => uv.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("login");
            entity.HasKey(uv => uv.Id);
            entity.Property(uv => uv.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");
            entity.Property(uv => uv.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid");
            entity.Property(uv => uv.Token).HasColumnName("token");
            entity.Property(uv => uv.ExpireAt).HasColumnName("expire_at");
            entity.Property(uv => uv.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<FriendsDbModel>(entity =>
        {
            entity.ToTable("friends");
            entity.HasKey(uv => uv.Id);
            entity.Property(uv => uv.Id).HasColumnName("id");
            entity.Property(uv => uv.UserId01).HasColumnName("user_id_01");
            entity.Property(uv => uv.UserId02).HasColumnName("user_id_02");
            entity.Property(uv => uv.Type).HasColumnName("type");
            entity.Property(uv => uv.Status).HasColumnName("status");
            entity.Property(uv => uv.CreatedAt).HasColumnName("created_at");
        });
        
        modelBuilder.Entity<UserView>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(uv => uv.Id);
            entity.Property(uv => uv.Id).HasColumnName("id");
            entity.Property(uv => uv.Name).HasColumnName("name");
            entity.Property(uv => uv.Email).HasColumnName("email");
            entity.Property(uv => uv.Country).HasColumnName("country");
            entity.Property(uv => uv.City).HasColumnName("city");
            entity.Property(uv => uv.Birthdate).HasColumnName("birthdate");
            entity.Property(uv => uv.Sexuality).HasColumnName("sexuality");
            entity.Property(uv => uv.SexualOrientation).HasColumnName("sexual_orientation");
            entity.Property(uv => uv.Password).HasColumnName("password");
            entity.Property(uv => uv.Height).HasColumnName("height");
            entity.Property(uv => uv.Weight).HasColumnName("weight");
            entity.Property(uv => uv.Latitude).HasColumnName("latitude");
            entity.Property(uv => uv.Longitude).HasColumnName("longitude");
            entity.Property(uv => uv.Status).HasColumnName("status");
        });
        
        modelBuilder.Entity<UserPhotoModel>(entity =>
        {
            entity.ToTable("user_photo");
            entity.HasKey(uv => uv.Id);
            entity.Property(uv => uv.Id).HasColumnName("id");
            entity.Property(uv => uv.UserId).HasColumnName("user_id");
            entity.Property(uv => uv.Url).HasColumnName("url");
            entity.Property(uv => uv.Tag).HasColumnName("tag");
            entity.Property(uv => uv.IsProfilePicture).HasColumnName("is_profile");
            entity.Property(uv => uv.CreatedAt).HasColumnName("created_at");
            
            entity.HasOne(uv => uv.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(uv => uv.UserId)
                .HasConstraintName("fk_userphoto_user") // opcional: nome da constraint no banco
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<PreferencesModel>(entity =>
        {
            entity.ToTable("user_preferences");
            entity.HasKey(uv => uv.Id);
            entity.Property(uv => uv.Id).HasColumnName("id");
            entity.Property(uv => uv.UserId).HasColumnName("user_id");
            entity.Property(uv => uv.Location).HasColumnName("location");
            entity.Property(uv => uv.DistanceKm).HasColumnName("distance_km");
            entity.Property(uv => uv.GenderPreference).HasColumnName("gender_preference");
            entity.Property(uv => uv.MinAge).HasColumnName("age_min");
            entity.Property(uv => uv.MaxAge).HasColumnName("age_max");
            entity.Property(uv => uv.MinHeight).HasColumnName("height_min");
            entity.Property(uv => uv.MaxHeight).HasColumnName("height_max");
            entity.Property(uv => uv.MinWeight).HasColumnName("weight_min");
            entity.Property(uv => uv.MaxWeight).HasColumnName("weight_max");
            entity.Property(uv => uv.Interests).HasColumnName("interests");
        });
        
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.Revoked).HasColumnName("is_revoked");
        });
    }
    
    public DbSet<UserView>? users_view { get; set; }
    public DbSet<UserView>? users { get; set; }
    public DbSet<Login>? login { get; set; }
    public DbSet<FriendsDbModel>? friends { get; set; }
    public DbSet<UserPhotoModel>? userPhotos { get; set; }
    public DbSet<PreferencesModel>? preferences { get; set; }
    public DbSet<RefreshToken>? refreshTokens { get; set; }
}