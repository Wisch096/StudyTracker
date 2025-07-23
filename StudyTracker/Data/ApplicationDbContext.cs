using Microsoft.EntityFrameworkCore;
using StudyTracker.Models;

namespace StudyTracker.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
      
    public DbSet<Activity> Activities { get; set; }
    public DbSet<WeeklyPlan> WeeklyPlans { get; set; }
      
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type)
                .HasConversion<string>() 
                .IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
              
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.Type);
        });
        
        modelBuilder.Entity<WeeklyPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WeekStartDate).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
              
            entity.HasIndex(e => e.WeekStartDate).IsUnique();
        });
    }
}