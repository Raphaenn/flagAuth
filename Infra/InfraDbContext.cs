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
        });
    }
    
    public DbSet<UserView>? users_view { get; set; }
    public DbSet<UserView>? users { get; set; }
    public DbSet<Login>? login { get; set; }
    public DbSet<FriendsDbModel>? friends { get; set; }
}