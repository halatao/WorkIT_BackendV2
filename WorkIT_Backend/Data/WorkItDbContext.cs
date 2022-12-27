using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Data;

public class WorkItDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Category>? Categories { get; set; }
    public DbSet<Location>? Locations { get; set; }
    public DbSet<Offer>? Offers { get; set; }
    public DbSet<Response>? Responses { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<User>? Users { get; set; }

    public WorkItDbContext()
    {
    }

    public WorkItDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        //base.Database.EnsureDeleted();
        //base.Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(q => q.CategoryId);
            entity.Property(q => q.CategoryId)
                .ValueGeneratedOnAdd();

            entity.Property(q => q.CategoryName)
                .IsRequired();
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(q => q.LocationId);
            entity.Property(q => q.LocationId)
                .ValueGeneratedOnAdd();

            entity.Property(q => q.LocationName)
                .IsRequired();
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(q => q.OfferId);
            entity.Property(q => q.OfferId)
                .ValueGeneratedOnAdd();

            entity.Property(q => q.OfferName)
                .IsRequired();

            entity.Property(q => q.OfferDescription)
                .IsRequired();

            entity.Property(q => q.SalaryMin);

            entity.Property(q => q.SalaryMax);

            entity.HasOne(o => o.Location)
                .WithMany(l => l.Offers)
                .HasForeignKey(o => o.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(o => o.Category)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(o => o.User)
                .WithMany(u => u.Offers)
                .HasForeignKey(o => o.UserId)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(q => q.ResponseId);
            entity.Property(q => q.ResponseId)
                .ValueGeneratedOnAdd();

            entity.Property(q => q.ResponseText)
                .IsRequired();

            entity.Property(q => q.CurriculumVitae);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(q => q.RoleId);
            entity.Property(q => q.RoleId)
                .ValueGeneratedOnAdd();

            entity.Property(q => q.Name)
                .IsRequired();
            entity.HasIndex(q => q.Name)
                .IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(q => q.UserId);
            entity.Property(q => q.UserId)
                .ValueGeneratedOnAdd();

            entity.Property(q => q.UserName)
                .IsRequired();

            entity.Property(q => q.PasswordHash)
                .IsRequired();

            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(u => u.Offers)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(u => u.Responses)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WorkItDb"));
    }
}